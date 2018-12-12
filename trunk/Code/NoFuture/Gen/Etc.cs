using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util;
using NoFuture.Util.Binary;
using NoFuture.Util.Core;
using NoFuture.Util.Gia.Args;
using NoFuture.Util.NfConsole;
using NoFuture.Util.NfType;

namespace NoFuture.Gen
{
    /// <summary>
    /// Main entry functions for code generation.
    /// </summary>
    public static class Etc
    {
        #region constants
        private const string DF_TYPE_NAME = "System.Object";
        private const string VALUE_TYPE = "ValueType";
        #endregion

        #region fields
        private static ArrayList _objectMembers;
        private static List<string> _ffVTypes;
        #endregion

        #region properties
        public static ArrayList ObjectMembers => _objectMembers ??
                                                 (_objectMembers =
                                                     new ArrayList {"Equals", "GetHashCode", "ToString", "GetType"});

        public static List<string> ValueTypesList
        {
            get
            {
                if (_ffVTypes == null)
                {
                    _ffVTypes = new List<string>();
                    _ffVTypes = NfReflect.ValueType2Cs.Keys.ToList();
                    _ffVTypes.Add("System.DateTime");
                    _ffVTypes.Add(Constants.ENUM);
                    _ffVTypes.AddRange(NfReflect.ValueType2Cs.Values);
                }
                return _ffVTypes;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Run its counterpart <see cref="GetClassDiagram"/> in an isolated process
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="typeFullName"></param>
        /// <param name="maxWaitInSeconds"></param>
        /// <returns></returns>
        public static string RunIsolatedGetClassDiagram(string assemblyPath, string typeFullName, int maxWaitInSeconds = 60)
        {
            var argPath = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_ASM_PATH_SWITCH, assemblyPath);
            var argType = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_FULL_TYPE_NAME_SWITCH, typeFullName);
            var diagramType = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_DIAGRAM_TYPE,
                Settings.CLASS_DIAGRAM);
            return RunGraphViz(argPath, argType, diagramType, maxWaitInSeconds);
        }

        /// <summary>
        /// Runs <see cref="NoFuture.Util.Gia.Flatten"/>
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="typeFullName"></param>
        /// <param name="maxDepth">The max recursive depth.</param>
        /// <param name="onlyPrimitivesNamed"></param>
        /// <param name="displayEnums"></param>
        /// <param name="maxWaitInSeconds"></param>
        /// <returns></returns>
        public static string RunIsolatedFlattenTypeDiagram(string assemblyPath, string typeFullName,
            string onlyPrimitivesNamed, bool displayEnums, int maxDepth = FlattenLineArgs.MAX_DEPTH, int maxWaitInSeconds = 60)
        {
            var argPath = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_ASM_PATH_SWITCH, assemblyPath);
            var argType = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_FULL_TYPE_NAME_SWITCH, typeFullName);
            var diagramType = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_DIAGRAM_TYPE,
                Settings.FLATTENED_DIAGRAM);
            var argP = !string.IsNullOrWhiteSpace(onlyPrimitivesNamed)
                ? ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_FLATTENED_LIMIT_TYPE, onlyPrimitivesNamed)
                : null;
            var argEn = displayEnums
                ? ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_DISPLAY_ENUMS, null)
                : null;
            var argMaxDepth = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_FLATTEN_MAX_DEPTH,
                maxDepth.ToString());
            return RunGraphViz(argPath, argType, diagramType, maxWaitInSeconds, argP, argEn, argMaxDepth);
        }

        /// <summary>
        /// Runs <see cref="NoFuture.Util.Gia.GraphViz.AsmDiagram"/> to generate a diagram 
        /// on another process to keep the loaded assemblies isolated from the invoking app domain.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="withNamespaceSubgraphs"></param>
        /// <returns></returns>
        public static string RunIsolatedAsmDiagram(string assemblyPath, bool withNamespaceSubgraphs = false)
        {
            var argPath = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_ASM_PATH_SWITCH, assemblyPath);
            var diagramType = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_DIAGRAM_TYPE,
                Settings.ASM_OBJ_GRAPH_DIAGRAM);
            if (withNamespaceSubgraphs)
            {
                var withNsOutlines = ConsoleCmd.ConstructCmdLineArgs(Settings.ASM_OBJ_OUTLINE_NS, null);
                return RunGraphViz(argPath, null, diagramType, 60, withNsOutlines);
            }

            return RunGraphViz(argPath, null, diagramType, 60);
        }

        /// <summary>
        /// Runs <see cref="NoFuture.Util.Gia.GraphViz.AsmDiagram"/> to generate a diagram 
        /// on another process to keep the loaded assemblies isolated from the invoking app domain.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns>This output is JSON and not GraphViz syntax</returns>
        public static string RunIsolatedAsmAdjGraph(string assemblyPath)
        {
            var argPath = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_ASM_PATH_SWITCH, assemblyPath);
            var diagramType = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_DIAGRAM_TYPE,
                Settings.ASM_ADJ_GRAPH);
            return RunGraphViz(argPath, null, diagramType, 60);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private static string RunGraphViz(string argPath, string argType, string diagramType, 
            int maxWaitInSeconds, params string[] additionalArgs)
        {
            var invokeGraphVizPath = NfConfig.CustomTools.InvokeGraphViz;
            if (string.IsNullOrWhiteSpace(invokeGraphVizPath))
                throw new RahRowRagee("The constants value at 'NoFuture.CustomTools.InvokeGraphViz' is not assigned.");
            if (!File.Exists(invokeGraphVizPath))
                throw new ItsDeadJim($"The binary 'NoFuture.Gen.InvokeGraphViz.exe' is not at '{invokeGraphVizPath}'.");

            string buffer = null;

            var args = new List<string> {argPath, argType, diagramType};
            foreach(var arg in additionalArgs.Where(x => !string.IsNullOrWhiteSpace(x)))
                args.Add(arg);

            using (var invokeGetCgType = new Process
            {
                StartInfo = new ProcessStartInfo(invokeGraphVizPath)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments = string.Join(" ", args.ToArray())
                }
            })
            {
                invokeGetCgType.Start();
                buffer = invokeGetCgType.StandardOutput.ReadToEnd();
                invokeGetCgType.WaitForExit(maxWaitInSeconds);

                if (!invokeGetCgType.HasExited)
                {
                    invokeGetCgType.Kill();
                    throw new ItsDeadJim(
                        "The 'NoFuture.Gen.InvokeGraphViz.exe' ran " +
                        $"longer than '{maxWaitInSeconds}' seconds and was shut down.");
                }
            }
            return buffer;
        }

        /// <summary>
        /// Specific to Graph-Viz (ver 2.38+) - produces the .gv file 
        /// to be compiled by dot.exe resulting in a class diagram of 
        /// the <see cref="assembly"/>
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static string GetClassDiagram(Assembly assembly, string typeFullName)
        {
            if(assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            if(string.IsNullOrWhiteSpace(typeFullName))
                throw new ArgumentNullException(nameof(typeFullName));
            var asmType = assembly.NfGetType(typeFullName);
            if (asmType == null)
                throw new RahRowRagee(
                    $"This type '{typeFullName}' could not be found in the assembly '{assembly.GetName().Name}'");

            var gv = new StringBuilder();
            var graphName = NfString.SafeDotNetIdentifier(assembly.GetName().Name);

            var topClass = GetCgOfType(assembly, typeFullName, false);
            var allCgTypeNames =
                topClass.AllPropertyTypes.Where(
                    x =>
                        !string.Equals(x, typeFullName, StringComparison.OrdinalIgnoreCase) &&
                        assembly.GetType(x) != null).ToList();

            var allCgTypes =
                allCgTypeNames.Where(x => !string.Equals(x, typeFullName, StringComparison.OrdinalIgnoreCase))
                    .Select(x => GetCgOfType(assembly, x, false))
                    .ToList();

            foreach (var aCgType in allCgTypes)
            {
                aCgType.IsEnum = topClass.Properties.FirstOrDefault(x => x.TypeName == aCgType.FullName && x.IsEnum) !=
                                 null;
            }

            //keep the diagram from exploding by multiples.
            var propertyManifold =
                allCgTypes.SelectMany(x => x.AllPropertyTypes)
                    .Where(
                        x =>
                            !string.Equals(x, typeFullName, StringComparison.OrdinalIgnoreCase) &&
                            !allCgTypeNames.Contains(x))
                    .Distinct()
                    .ToList();

            //determine which types require an empty class node
            var missingTypes =
                allCgTypes.SelectMany(x => x.AllPropertyTypes).Where(x => assembly.NfGetType(x) == null).Distinct().ToList();

            missingTypes.AddRange(
                topClass.AllPropertyTypes.Where(x => assembly.NfGetType(x) == null && !missingTypes.Contains(x)));

            gv.Append($"digraph {graphName}Assembly");
            gv.AppendLine("{");
            gv.AppendLine("graph [rankdir=\"LR\"];");
            gv.AppendLine("node [fontname=\"Consolas\"];");

            gv.AppendLine(topClass.ToGraphVizNode());

            foreach (var missing in missingTypes)
                gv.AppendLine(GraphVizExtensions.EmptyGraphVizClassNode(missing,topClass.EnumsValues(missing)));

            foreach (var far in propertyManifold)
                gv.AppendLine(GraphVizExtensions.EmptyGraphVizClassNode(far, topClass.EnumsValues(far)));

            foreach (var cg in allCgTypes.Where(x => !x.IsEnum))
                gv.AppendLine(cg.ToGraphVizNode());

            foreach (
                var cg in
                    allCgTypes.Where(
                        x => x.IsEnum && !missingTypes.Contains(x.FullName) && !propertyManifold.Contains(x.FullName)))
                gv.AppendLine(GraphVizExtensions.EmptyGraphVizClassNode(cg.FullName, topClass.EnumsValues(cg.FullName)));

            gv.AppendLine();
            gv.AppendLine();

            gv.AppendLine(topClass.ToGraphVizEdge());

            foreach (var cg in allCgTypes)
                gv.AppendLine(cg.ToGraphVizEdge());

            gv.AppendLine("}");
            return gv.ToString();
        }

        /// <summary>
        /// Runs its counterpart <see cref="GetCgOfType"/> in an isolated process.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="typeFullName"></param>
        /// <param name="resolveDependencies"></param>
        /// <param name="maxWaitInSeconds"></param>
        /// <returns></returns>
        public static CgType GetIsolatedCgOfType(string assemblyPath, string typeFullName, 
            bool resolveDependencies = false, int maxWaitInSeconds = 60)
        {
            var tempDebug = NfConfig.TempDirectories.Debug;
            if(string.IsNullOrWhiteSpace(tempDebug) || !Directory.Exists(tempDebug))
                throw new RahRowRagee("The constant value at " +
                                      "'NoFuture.Shared.Cfg.NfConfig.TempDirectories.Debug' is not assigned.");

            var invokeGetCgTypePath = NfConfig.CustomTools.InvokeGetCgType;

            if(string.IsNullOrWhiteSpace(typeFullName))
                throw new ArgumentNullException(nameof(typeFullName));
            if(string.IsNullOrWhiteSpace(invokeGetCgTypePath))
                throw new RahRowRagee("The constants value at " +
                                      "'NoFuture.Shared.Cfg.NfConfig.CustomTools.InvokeGetCgType' is not assigned.");
            if(!File.Exists(invokeGetCgTypePath))
                throw new ItsDeadJim(
                    $"The binary 'NoFuture.Gen.InvokeGetCgOfType.exe' is not at '{invokeGetCgTypePath}'.");

            var argPath = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_ASM_PATH_SWITCH, assemblyPath);
            var argType = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_FULL_TYPE_NAME_SWITCH, typeFullName);
            var argResolve = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_RESOLVE_ALL_DEPENDENCIES,
                resolveDependencies.ToString());
            string buffer = null;

            using (var invokeGetCgType = new Process
            {
                StartInfo = new ProcessStartInfo(invokeGetCgTypePath)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments = string.Join(" ", new[] { argPath, argType, argResolve })
                }
            })
            {
                invokeGetCgType.Start();
                buffer = invokeGetCgType.StandardOutput.ReadToEnd();
                invokeGetCgType.WaitForExit(maxWaitInSeconds);

                if (!invokeGetCgType.HasExited)
                {
                    invokeGetCgType.Kill();
                    throw new ItsDeadJim(
                        "The 'NoFuture.Gen.InvokeGetCgOfType.exe' ran " +
                        $"longer than '{maxWaitInSeconds}' seconds and was shut down.");
                }
            }

            if(string.IsNullOrWhiteSpace(buffer))
                throw new ItsDeadJim("The invocation to 'NoFuture.Gen.InvokeGetCgOfType.exe' didn't " +
                                     "return anything on its standard output.");

            File.WriteAllBytes(Path.Combine(tempDebug, "GetIsolatedCgType.json"),
                Encoding.UTF8.GetBytes(buffer));

            var dcs = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(CgType));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(buffer)))
            {
                var cgTypeOut = dcs.ReadObject(ms) as CgType;
                if(cgTypeOut == null)
                    throw new ItsDeadJim(
                        $"Could not deserialize into a CgType from the standard output text of\n {buffer}");
                return cgTypeOut;
            }
        }

        /// <summary>
        /// Get a custom type specific for Code Gen purposes based on the 
        /// given <see cref="typeFullName"/>
        /// </summary>
        /// <param name="assembly">A reference to the assembly in which the type is located.</param>
        /// <param name="typeFullName">The type which will produce the cgObject.</param>
        /// <param name="valueTypeOnly">Will only export Fields and Properties whose base type extends System.ValueType</param>
        /// <param name="resolveDependencies">Switch to have the IL of the type parsed and all dependent calls Metadata tokens added.</param>
        /// <returns></returns>
        public static CgType GetCgOfType(Assembly assembly, string typeFullName, 
            bool valueTypeOnly, bool resolveDependencies = false)
        {
            if (assembly == null || string.IsNullOrWhiteSpace(typeFullName))
                return null;
           
            var asmType = assembly.NfGetType(typeFullName);
            var cgType = GetCgOfType(asmType, valueTypeOnly,resolveDependencies);
            return cgType;
        }

        /// <summary>
        /// Converts the .NET type into the custom Code Gen type
        /// </summary>
        /// <param name="asmType"></param>
        /// <param name="valueTypeOnly">
        /// Will only export Fields and Properties whose base type extends System.ValueType
        /// </param>
        /// <param name="resolveDependencies">
        /// Switch to have the IL of the type parsed and all dependent calls Metadata tokens added.
        /// </param>
        /// <returns></returns>
        public static CgType GetCgOfType(this Type asmType, bool valueTypeOnly, bool resolveDependencies = false)
        {
            var cgType = new CgType();

            if (asmType == null || NfReflect.IsIgnoreType(asmType) || NfReflect.IsClrGeneratedType(asmType))
                return null;
            
            //use the logic in TypeName to get the namespace and class name so its not tied to having the assembly
            var cgTypeName = new NfTypeName(asmType.AssemblyQualifiedName);

            //make sure there is always some kind of namespace
            cgType.Namespace = string.IsNullOrWhiteSpace(cgTypeName.Namespace)
                ? asmType.Assembly.GetName().Name
                : cgTypeName.Namespace;
            cgType.IsContrivedNamespace = string.IsNullOrWhiteSpace(cgTypeName.Namespace);
            cgType.Name = cgTypeName.ClassName;
            cgType.AssemblyQualifiedName = asmType.AssemblyQualifiedName;
            cgType.IsEnum = NfReflect.IsEnumType(asmType);

            var cgTypesInterfaces = asmType.GetInterfaces();
            cgType.MetadataToken = asmType.MetadataToken;

            Func<CgType, string, bool> alreadyPresentHerein =
                (cg, nextName) =>
                    (cg.Properties.FirstOrDefault(
                        cgP => string.Equals(cgP.Name, nextName, StringComparison.OrdinalIgnoreCase)) != null
                     ||
                     cg.Fields.FirstOrDefault(
                         cgF => string.Equals(cgF.Name, nextName, StringComparison.OrdinalIgnoreCase)) != null
                     ||
                     cg.Events.FirstOrDefault(
                         cgE => string.Equals(cgE.Name, nextName, StringComparison.OrdinalIgnoreCase)) != null)
                ;

            //have events go first since they tend to be speard across fields and properties
            foreach (
                var evtInfo in
                    asmType.GetEvents(Shared.Core.NfSettings.DefaultFlags))
            {
                var evtHandlerType = evtInfo.NfEventHandlerType().ToString();

                var cgMem = new CgMember
                {
                    Name = evtInfo.Name,
                    TypeName = evtHandlerType,
                    MetadataToken = evtInfo.MetadataToken
                };
                cgType.Events.Add(cgMem);
            }

            var asmMembers =
                asmType.GetMembers(Shared.Core.NfSettings.DefaultFlags);

            foreach (var mi in asmMembers)
            {
                if (alreadyPresentHerein(cgType, mi.Name))
                    continue;

                try
                {
                    if (mi.MemberType == MemberTypes.Property)
                    {
                        var pi = mi as PropertyInfo;

                        var cgm = GetAsCgMember(pi, valueTypeOnly, cgType.EnumValueDictionary);
                        if (cgm == null)
                            continue;
                        if (resolveDependencies)
                        {
                            var propMi = NfReflect.GetMethodsForProperty(pi, asmType);
                            foreach (var pim in propMi)
                            {
                                cgm.opCodeCallsAndCallvirtsMetadatTokens.AddRange(Asm.GetCallsMetadataTokens(pim));
                            }
                        }
                        cgType.Properties.Add(cgm);
                    }
                }
                catch (Exception ex)
                {
                    Asm.AddLoaderExceptionToLog(null, ex);

                    if (!Settings.IgnoreReflectionMissingAsmError)
                        throw;

                    cgType.Properties.Add(new CgMember
                    {
                        Name = mi.Name,
                        TypeName = DF_TYPE_NAME,
                        HasGetter = true,
                        HasSetter = true,
                        SkipIt = true
                    });
                }

                try
                {
                    if (mi.MemberType == MemberTypes.Event)
                    {
                        continue;//these have already been handled
                    }
                }
                catch (Exception ex)
                {
                    Asm.AddLoaderExceptionToLog(null, ex);
                    continue;
                }

                try
                {
                    if (mi.MemberType == MemberTypes.Field)
                    {
                        var fi = mi as FieldInfo;
                        var cgm = GetAsCgMember(fi, valueTypeOnly, cgType.EnumValueDictionary);

                        if (cgm == null)
                            continue;
                        cgType.Fields.Add(cgm);
                    }
                }
                catch (Exception ex)
                {
                    Asm.AddLoaderExceptionToLog(null, ex);
                    if (!Settings.IgnoreReflectionMissingAsmError)
                        throw;

                    cgType.Fields.Add(new CgMember
                    {
                        Name = mi.Name,
                        TypeName = DF_TYPE_NAME,
                        SkipIt = true
                    });
                }
                try
                {
                    if (!valueTypeOnly && mi.MemberType == MemberTypes.Method)
                    {
                        var mti = mi as MethodInfo;
                        var cgm = GetAsCgMember(mti, resolveDependencies);

                        if (cgm == null)
                            continue;
                        cgm.IsInterfaceImpl = IsInterfaceImpl(mti, cgTypesInterfaces);
                        cgType.Methods.Add(cgm);
                    }

                    if (!valueTypeOnly && mi.MemberType == MemberTypes.Constructor)
                    {
                        var ci = mi as ConstructorInfo;
                        var tn = (string.IsNullOrWhiteSpace(cgTypeName.Namespace)
                            ? cgTypeName.ClassName
                            : $"{cgTypeName.Namespace}.{cgTypeName.ClassName}");

                        var cgm = GetAsCgMember(ci, tn, resolveDependencies);

                        if (cgm == null)
                            continue;
                        cgType.Methods.Add(cgm);
                    }

                }
                catch (Exception ex)
                {
                    Asm.AddLoaderExceptionToLog(null, ex);
                    if (!Settings.IgnoreReflectionMissingAsmError)
                        throw;

                    cgType.Methods.Add(new CgMember
                    {
                        Name = mi.Name,
                        TypeName = DF_TYPE_NAME,
                        SkipIt = true
                    });
                }
            }

            if (resolveDependencies)
            {
                ResolveAllMetadataTokens(cgType, asmType.Assembly.ManifestModule);
            }

            return cgType;
        }

        /// <summary>
        /// Performs a heavy task of resolving every MetadataToken in all <see cref="cgType"/>'s Methods and Properties.
        /// </summary>
        /// <param name="cgType"></param>
        /// <param name="manifestModule"></param>
        public static void ResolveAllMetadataTokens(CgType cgType, Module manifestModule)
        {
            if (cgType == null || manifestModule == null)
                return;
            //improve performance for common tokens invoked in lots of members.
            var memberInfoCache = new Dictionary<int, MemberInfo>();

            //we don't want this clutter
            var gacAssemblies = NfConfig.UseReflectionOnlyLoad
                ? AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                    .Where(x => x.GlobalAssemblyCache)
                    .Select(x => x.FullName)
                    .ToArray()
                : AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.GlobalAssemblyCache)
                    .Select(x => x.FullName)
                    .ToArray();

            //go through each method on the cgType
            foreach (var cgm in cgType.Methods)
            {
                ResolveCgMemberMetadataTokens(cgm, manifestModule, memberInfoCache, gacAssemblies);
            }

            //then go through each property
            foreach (var cgm in cgType.Properties)
            {
                ResolveCgMemberMetadataTokens(cgm, manifestModule, memberInfoCache, gacAssemblies);
            }
        }

        /// <summary>
        /// Resolves all MetadataTokens present in <see cref="cgm"/> using the <see cref="manifestModule"/>
        /// </summary>
        /// <param name="cgm"></param>
        /// <param name="manifestModule"></param>
        /// <param name="memberInfoCache">
        /// Optional cache dictionary for use when this is being called within a loop, helps performance.
        /// </param>
        /// <param name="ignoreAssemblyFullNames">
        /// Optional list of fully-qualified assembly names whose members should NOT be added (e.g. mscorlib).
        /// </param>
        /// <param name="maxErrors">The max number of exceptions to be incurred before abandoning.</param>
        /// <returns>Count of errors encountered, will range from 0 to <see cref="maxErrors"/></returns>
        public static int ResolveCgMemberMetadataTokens(CgMember cgm, Module manifestModule,
            Dictionary<int, MemberInfo> memberInfoCache, string[] ignoreAssemblyFullNames, int maxErrors = 5)
        {
            if (cgm == null)
                return maxErrors;
            if (manifestModule == null)
                return maxErrors;

            var errorCount = 0;
            //for each method, get all invoked metadata tokens
            foreach (var metadataToken in cgm.opCodeCallsAndCallvirtsMetadatTokens)
            {
                try
                {
                    MemberInfo runtimeMem = null;
                    //resolve invoked metadata token to a member type
                    if (memberInfoCache != null && memberInfoCache.ContainsKey(metadataToken))
                    {
                        runtimeMem = memberInfoCache[metadataToken];
                    }
                    else
                    {
                        runtimeMem = manifestModule.ResolveMember(metadataToken);
                    }
                    if (runtimeMem == null)
                        continue;
                    var declaringType = runtimeMem.DeclaringType;
                    var declaringTypeAsmName = declaringType?.AssemblyQualifiedName;

                    if (string.IsNullOrWhiteSpace(declaringTypeAsmName))
                        continue;

                    if (ignoreAssemblyFullNames != null && ignoreAssemblyFullNames.Any(declaringTypeAsmName.EndsWith))
                        continue;

                    //add to cache for performance
                    if (memberInfoCache != null && !memberInfoCache.ContainsKey(metadataToken))
                        memberInfoCache.Add(metadataToken, runtimeMem);

                    var runtimeMi = runtimeMem as MethodInfo;
                    if (runtimeMi != null)
                    {
                        var ncgm = GetAsCgMember(runtimeMi, false);
                        if (ncgm == null)
                            continue;
                        ncgm.MetadataToken = metadataToken;
                        ncgm.DeclaringTypeAsmName = declaringTypeAsmName;

                        cgm.OpCodeCallAndCallvirts.Add(ncgm);
                        continue;
                    }

                    var runtimePi = runtimeMem as PropertyInfo;
                    if (runtimePi != null)
                    {
                        var ncgm = GetAsCgMember(runtimePi, false, null);
                        if (ncgm == null)
                            continue;

                        ncgm.MetadataToken = metadataToken;
                        ncgm.DeclaringTypeAsmName = declaringTypeAsmName;
                        cgm.OpCodeCallAndCallvirts.Add(ncgm);
                        continue;
                    }

                    var runtimeFi = runtimeMem as FieldInfo;
                    if (runtimeFi == null)
                        continue;

                    var fiCgm = GetAsCgMember(runtimeFi, false, null);
                    if (fiCgm == null)
                        continue;
                    fiCgm.MetadataToken = metadataToken;
                    fiCgm.DeclaringTypeAsmName = declaringTypeAsmName;
                    cgm.OpCodeCallAndCallvirts.Add(fiCgm);
                }
                catch
                {
                    errorCount += 1;
                    if (errorCount > maxErrors)
                        break;
                }
            }
            return errorCount;
        }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> as a code gen member (<see cref="CgMember"/>)
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="valueTypeOnly"></param>
        /// <param name="enumValueDictionary"></param>
        /// <returns></returns>
        public static CgMember GetAsCgMember(PropertyInfo pi, bool valueTypeOnly,
            Dictionary<string, string[]> enumValueDictionary)
        {
            if (pi == null)
                return null;

            var piType = pi.NfPropertyType();

            if (valueTypeOnly && piType.NfBaseType() != null &&
                piType.NfBaseType().Name != VALUE_TYPE)
                return null;

            var cgMem = new CgMember
            {
                TypeName = Settings.LangStyle.TransformClrTypeSyntax(piType),
                IsEnumerableType = NfReflect.IsEnumerableReturnType(piType),
                Name = pi.Name,
                MetadataToken = pi.MetadataToken
            };
            if (pi.CanRead)
            {
                cgMem.HasGetter = true;
                if (pi.NfGetGetMethod() != null)
                    cgMem.IsStatic = pi.NfGetGetMethod().IsStatic;
            }

            if (pi.CanWrite)
            {
                cgMem.HasSetter = true;
                if (pi.NfGetSetMethod() != null)
                    cgMem.IsStatic = pi.NfGetSetMethod().IsStatic;
            }

            string[] enumVals;
            if (NfReflect.IsEnumType(piType, out enumVals) && enumValueDictionary != null)
            {
                cgMem.IsEnum = true;
                var clearName = NfReflect.GetLastTypeNameFromArrayAndGeneric(cgMem.TypeName, "<");
                if (!enumValueDictionary.ContainsKey(clearName))
                {
                    enumValueDictionary.Add(clearName, enumVals);
                }
            }

            return cgMem;
        }

        /// <summary>
        /// Gets the <see cref="FieldInfo"/> as a code gen member (<see cref="CgMember"/>)
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="valueTypeOnly"></param>
        /// <param name="enumValueDictionary"></param>
        /// <returns></returns>
        public static CgMember GetAsCgMember(FieldInfo fi, bool valueTypeOnly,
            Dictionary<string, string[]> enumValueDictionary)
        {
            if (fi == null)
                return null;

            if (NfReflect.IsClrGeneratedType(fi.Name))
                return null;

            var fiType = fi.NfFieldType();

            if (valueTypeOnly && fiType.NfBaseType() != null && fiType.NfBaseType().Name != VALUE_TYPE)
                return null;

            var cgMem = new CgMember
            {
                TypeName = Settings.LangStyle.TransformClrTypeSyntax(fiType),
                IsEnumerableType = NfReflect.IsEnumerableReturnType(fiType),
                Name = fi.Name,
                IsStatic = fi.IsStatic,
                MetadataToken = fi.MetadataToken
            };

            string[] enumVals;
            if (NfReflect.IsEnumType(fiType, out enumVals) && enumValueDictionary != null)
            {
                cgMem.IsEnum = true;
                var clearName = NfReflect.GetLastTypeNameFromArrayAndGeneric(cgMem.TypeName, "<");
                if (!enumValueDictionary.ContainsKey(clearName))
                {
                    enumValueDictionary.Add(clearName, enumVals);
                }
            }
            return cgMem;
        }

        /// <summary>
        /// Gets a constructor as a code gen member type
        /// </summary>
        /// <param name="ci"></param>
        /// <param name="typeName"></param>
        /// <param name="getCallvirtMetadataTokens"></param>
        /// <returns></returns>
        public static CgMember GetAsCgMember(ConstructorInfo ci, string typeName, bool getCallvirtMetadataTokens = false)
        {
            if (ci == null || string.IsNullOrWhiteSpace(typeName))
                return null;

            var cgMem = new CgMember
            {
                Name = NfConfig.CTOR_NAME,
                IsCtor = true,
                TypeName = typeName,
                MetadataToken = ci.MetadataToken
            };
            foreach (var parameterInfo in ci.GetParameters())
            {
                var paramType = parameterInfo.NfParameterType();
                var cgArg = new CgArg
                {
                    ArgName = parameterInfo.Name,
                    ArgType = Settings.LangStyle.TransformClrTypeSyntax(paramType)
                };

                cgMem.Args.Add(cgArg);
            }

            if (!getCallvirtMetadataTokens) return cgMem;

            cgMem.opCodeCallsAndCallvirtsMetadatTokens.AddRange(Asm.GetCallsMetadataTokens(ci));
            return cgMem;
        }

        /// <summary>
        /// Gets a method as a code gen member type
        /// </summary>
        /// <param name="mti"></param>
        /// <param name="getCallvirtMetadataTokens"></param>
        /// <returns></returns>
        public static CgMember GetAsCgMember(MethodInfo mti, bool getCallvirtMetadataTokens = false)
        {
            if (mti == null)
                return null;
            string propName;
            if (NfReflect.IsClrMethodForProperty(mti.Name, out propName))
                return null;
            if (NfReflect.IsClrGeneratedType(mti.Name))
                //these appear as '<SomeNameOfMethodAlreadyAdded>b__kifkj(...)'
                return null;

            var miReturnType = mti.NfReturnType();

            var cgMem = new CgMember
            {
                Name = mti.Name,
                TypeName = Settings.LangStyle.TransformClrTypeSyntax(miReturnType),
                IsStatic = mti.IsStatic,
                IsGeneric = mti.IsGenericMethod,
                IsEnumerableType = NfReflect.IsEnumerableReturnType(miReturnType),
                IsMethod = true,
                MetadataToken = mti.MetadataToken
            };
            if(mti.IsAssembly)
                cgMem.AccessModifier = CgAccessModifier.Assembly;
            if(mti.IsFamily)
                cgMem.AccessModifier = CgAccessModifier.Family;
            if(mti.IsFamilyAndAssembly)
                cgMem.AccessModifier = CgAccessModifier.FamilyAssembly;
            if(mti.IsPrivate)
                cgMem.AccessModifier = CgAccessModifier.Private;
            if(mti.IsPublic)
                cgMem.AccessModifier = CgAccessModifier.Public;

            foreach (var parameterInfo in mti.GetParameters())
            {
                var paramType = parameterInfo.NfParameterType();
                var cgArg = new CgArg
                {
                    ArgName = parameterInfo.Name,
                    ArgType = Settings.LangStyle.TransformClrTypeSyntax(paramType)
                };

                cgMem.Args.Add(cgArg);
            }
            cgMem.MethodBodyIL = Convert.ToBase64String(Asm.GetMethodBody(mti));

            if (!getCallvirtMetadataTokens)
                return cgMem;

            cgMem.opCodeCallsAndCallvirtsMetadatTokens.AddRange(Asm.GetCallsMetadataTokens(mti));
            return cgMem;
        }

        //https://msdn.microsoft.com/en-us/library/system.reflection.methodinfo.getbasedefinition(v=vs.110).aspx
        internal static bool IsInterfaceImpl(MethodInfo mti, Type[] cgTypesInterfaces)
        {
            if (mti == null)
                return false;
            if (cgTypesInterfaces == null || cgTypesInterfaces.Length <= 0)
                return false;
            try
            {
                var bdef = mti.GetBaseDefinition();
                if (bdef == null)
                    return false;
                //If the current MethodInfo object represents an interface implementation, 
                //the GetBaseDefinition method returns the current MethodInfo object.
                if (!bdef.Equals(mti))
                    return false;

                foreach (var i in cgTypesInterfaces)
                {
                    var iMethods = i.GetMethods();

                    foreach (var im in iMethods)
                    {
                        if (im.Name != mti.Name)
                            continue;
                        var imParams = im.GetParameters();
                        var mtiParams = mti.GetParameters();

                        if (imParams.Length != mtiParams.Length)
                            continue;

                        if (mti.NfReturnType() != im.NfReturnType())
                            return false;

                        if (imParams.Length == 0)
                            return true;

                        var imParamTypes = imParams.Select(x => x.NfParameterType()).ToList();
                        var mtiParamTypes = mtiParams.Select(x => x.NfParameterType()).ToList();

                        return imParamTypes.All(x => mtiParamTypes.Any(y => x == y));
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }


        /// <summary>
        /// Used in <see cref="ToPlural"/> for nouns which posses no discernable pattern
        /// for being rendered plural.
        /// </summary>
        public static readonly Hashtable IRREGULAR_PLURALS = new Hashtable()
        {
            {"Woman", "Women"},
            {"Man", "Men"},
            {"Child", "Children"},
            {"Person", "People"},
            {"Datum", "Data" },
            {"Foot", "Feet" },
            {"Goose","Geese" },
            {"Tooth","Teeth" },
            {"Mouse","Mice" },
            {"Deer", "Deer" },
            {"Moose", "Moose" },
        };

        /// <summary>
        /// Puralize an English word - pays no respect to if word may already 
        /// be the plural version of itself, nor if the word is actually a noun.
        /// </summary>
        /// <param name="name">Expected to be a singular noun.</param>
        /// <param name="skipWordsEndingInS">Set this to true to break out when words already end with 's'.</param>
        /// <remarks>
        /// The command handles some diphthong (e.g. Tooth -> Teeth).
        /// </remarks>
        /// <returns></returns>
        public static string ToPlural(string name, bool skipWordsEndingInS = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            name = name.Trim();

            if (name.Length <= 1)
                return name;

            if (name.EndsWith("s") && skipWordsEndingInS)
                return name;

            if (name.EndsWith("us") && name.Length > 2)
            {
                return $"{name.Substring(0, name.Length - 2)}i";
            }

            if ((name.EndsWith("s") || name.EndsWith("ch") || name.EndsWith("o")) && !name.EndsWith("es"))
            {
                return $"{name}es";
            }
            if (name.EndsWith("y"))
            {
                var nameArray = name.ToCharArray();
                var secondToLastIndex = name.Length - 2;
                if (secondToLastIndex > 0)
                {
                    if (!Regex.IsMatch(nameArray[secondToLastIndex].ToString(CultureInfo.InvariantCulture), "[aeiou]"))
                    {

                        return $"{name.Substring(0, name.Length - 1)}ies";
                    }
                }
            }
            if (name.EndsWith("fe") && name.Length > 2)
            {
                return $"{name.Substring(0, name.Length - 2)}ves";
            }
            if ((name.EndsWith("lf") || name.EndsWith("af") || name.EndsWith("ef")) && name.Length > 2)
            {
                return $"{name.Substring(0, name.Length - 1)}ves";
            }
            if (name.EndsWith("x"))
            {
                return $"{name.Substring(0, name.Length - 1)}ces";
            }

            if ((name.EndsWith("on") || name.EndsWith("um")) && name.Length > 2)
            {
                return $"{name.Substring(0, name.Length - 2)}a";
            }

            if (IRREGULAR_PLURALS.ContainsKey(name))
            {
                return IRREGULAR_PLURALS[name].ToString();
            }

            foreach (var val in IRREGULAR_PLURALS.Values)
            {
                if (val == null)
                    continue;
                if (name.EndsWith(val.ToString()))
                    return name;
            }

            return $"{name}s";
        }
        #endregion
    }
}

