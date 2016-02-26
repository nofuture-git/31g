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
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util;
using NoFuture.Tools;
using NoFuture.Util.Binary;

namespace NoFuture.Gen
{
    /// <summary>
    /// Main entry functions for code generation.
    /// </summary>
    public class Etc
    {
        #region private static fields
        private static ArrayList _objectMembers;
        private static List<string> _ffVTypes;
        #endregion

        public static ArrayList ObjectMembers
        {
            get { return _objectMembers ?? (_objectMembers = new ArrayList {"Equals", "GetHashCode", "ToString", "GetType"}); }
        }

        public static List<string> ValueTypesList
        {
            get
            {
                if (_ffVTypes == null)
                {
                    _ffVTypes = new List<string>();
                    _ffVTypes = Lexicon.ValueType2Cs.Keys.ToList();
                    _ffVTypes.Add("System.DateTime");
                    _ffVTypes.Add(Constants.ENUM);
                    _ffVTypes.AddRange(Lexicon.ValueType2Cs.Values);
                }
                return _ffVTypes;
            }
        }

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
            return RunGraphViz(argPath, argType, diagramType, null, null, maxWaitInSeconds);
        }

        /// <summary>
        /// Runs <see cref="NoFuture.Util.Gia.Flatten.FlattenType"/>
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="typeFullName"></param>
        /// <param name="onlyPrimitivesNamed"></param>
        /// <param name="displayEnums"></param>
        /// <param name="maxWaitInSeconds"></param>
        /// <returns></returns>
        public static string RunIsolatedFlattenTypeDiagram(string assemblyPath, string typeFullName, string onlyPrimitivesNamed, bool displayEnums, int maxWaitInSeconds = 60)
        {
            var argPath = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_ASM_PATH_SWITCH, assemblyPath);
            var argType = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_FULL_TYPE_NAME_SWITCH, typeFullName);
            var diagramType = ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_DIAGRAM_TYPE,
                Settings.FLATTENED_DIAGRAM);
            var argP = !string.IsNullOrWhiteSpace(onlyPrimitivesNamed)
                ? ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_FLATTENED_LIMIT_TYPE, onlyPrimitivesNamed)
                : null;
            var argEn = displayEnums ? ConsoleCmd.ConstructCmdLineArgs(Settings.INVOKE_GRAPHVIZ_DISPLAY_ENUMS, null) : null;
            return RunGraphViz(argPath, argType, diagramType, argP, argEn, maxWaitInSeconds);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private static string RunGraphViz(string argPath, string argType, string diagramType, string primitiveLimit, string displayEnumsArg, int maxWaitInSeconds)
        {
            var invokeGraphVizPath = CustomTools.InvokeGraphViz;
            if (string.IsNullOrWhiteSpace(invokeGraphVizPath))
                throw new RahRowRagee("The constants value at 'NoFuture.CustomTools.InvokeGraphViz' is not assigned.");
            if (!File.Exists(invokeGraphVizPath))
                throw new ItsDeadJim(string.Format("The binary 'NoFuture.Gen.InvokeGraphViz.exe' is not at '{0}'.", invokeGraphVizPath));

            string buffer = null;

            var args = new List<string> {argPath, argType, diagramType};
            if(!string.IsNullOrWhiteSpace(primitiveLimit))
                args.Add(primitiveLimit);
            if(!string.IsNullOrWhiteSpace(displayEnumsArg))
                args.Add(displayEnumsArg);

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
                    throw new ItsDeadJim(string.Format("The 'NoFuture.Gen.InvokeGraphViz.exe' ran longer than '{0}' seconds and was shut down.", maxWaitInSeconds));
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
                throw new ArgumentNullException("assembly");
            if(string.IsNullOrWhiteSpace(typeFullName))
                throw new ArgumentNullException("typeFullName");
            var asmType = assembly.NfGetType(typeFullName);
            if (asmType == null)
                throw new RahRowRagee(string.Format("This type '{0}' could not be found in the assembly '{1}'", typeFullName, assembly.GetName().Name));

            var gv = new StringBuilder();
            var graphName = NfTypeName.SafeDotNetIdentifier(assembly.GetName().Name);

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

            gv.AppendFormat("digraph {0}Assembly", graphName);
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
        public static CgType GetIsolatedCgOfType(string assemblyPath, string typeFullName, bool resolveDependencies = false, int maxWaitInSeconds = 60)
        {
            var invokeGetCgTypePath = CustomTools.InvokeGetCgType;

            if(string.IsNullOrWhiteSpace(typeFullName))
                throw new ArgumentNullException("typeFullName");
            if(string.IsNullOrWhiteSpace(invokeGetCgTypePath))
                throw new RahRowRagee("The constants value at 'NoFuture.CustomTools.InvokeGetCgType' is not assigned.");
            if(!File.Exists(invokeGetCgTypePath))
                throw new ItsDeadJim(string.Format("The binary 'NoFuture.Gen.InvokeGetCgOfType.exe' is not at '{0}'.", invokeGetCgTypePath));

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
                    throw new ItsDeadJim(string.Format("The 'NoFuture.Gen.InvokeGetCgOfType.exe' ran longer than '{0}' seconds and was shut down.",maxWaitInSeconds));
                }
            }

            if(string.IsNullOrWhiteSpace(buffer))
                throw new ItsDeadJim("The invocation to 'NoFuture.Gen.InvokeGetCgOfType.exe' didn't return anything on its standard output.");

            File.WriteAllBytes(Path.Combine(TempDirectories.Debug, "GetIsolatedCgType.json"), Encoding.UTF8.GetBytes(buffer));

            var dcs = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(CgType));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(buffer)))
            {
                var cgTypeOut = dcs.ReadObject(ms) as CgType;
                if(cgTypeOut == null)
                    throw new ItsDeadJim(string.Format("Could not deserialize into a CgType from the standard output text of\n {0}",buffer));
                return cgTypeOut;
            }
        }

        /// <summary>
        /// Given the name of some type which is present within the 
        /// given assembly, the cmdlet will produce a psobject to be 
        /// used in one of the code generation cmdlets
        /// </summary>
        /// <param name="assembly">A reference to the assembly in which the type is located.</param>
        /// <param name="typeFullName">The type which will produce the cgObject.</param>
        /// <param name="valueTypeOnly">Will only export Fields and Properties whose base type extends System.ValueType</param>
        /// <param name="resolveDependencies">Switch to have the IL of the type parsed and all dependent calls Metadata tokens added.</param>
        /// <returns></returns>
        public static CgType GetCgOfType(Assembly assembly, string typeFullName, bool valueTypeOnly, bool resolveDependencies = false)
        {
            if (assembly == null || String.IsNullOrWhiteSpace(typeFullName))
                return null;
            var cgType = new CgType();
            var asmType = assembly.NfGetType(typeFullName);

            if (asmType == null || NfTypeName.IsIgnoreType(asmType) || NfTypeName.IsClrGeneratedType(asmType))
                return null;

            //use the logic in TypeName to get the namespace and class name so its not tied to having the assembly
            var cgTypeName = new NfTypeName(asmType.AssemblyQualifiedName);

            //make sure there is always some kind of namespace
            cgType.Namespace = string.IsNullOrWhiteSpace(cgTypeName.Namespace)
                ? assembly.GetName().Name
                : cgTypeName.Namespace;
            cgType.IsContrivedNamespace = string.IsNullOrWhiteSpace(cgTypeName.Namespace);
            cgType.Name = cgTypeName.ClassName;
            cgType.AssemblyQualifiedName = asmType.AssemblyQualifiedName;
            cgType.IsEnum = NfTypeName.IsEnumType(asmType);

            var cgTypesInterfaces = asmType.GetInterfaces();
            cgType.MetadataToken = asmType.MetadataToken;

            Func<CgType, string, bool> alreadyPresentHerein =
                (cg, nextName) =>  (cg.Properties.FirstOrDefault(cgP => string.Equals(cgP.Name, nextName, StringComparison.OrdinalIgnoreCase)) != null 
                                    || cg.Fields.FirstOrDefault(cgF => string.Equals(cgF.Name, nextName,StringComparison.OrdinalIgnoreCase)) != null 
                                    || cg.Events.FirstOrDefault(cgE => string.Equals(cgE.Name, nextName, StringComparison.OrdinalIgnoreCase)) != null)
                                   ;

            //have events go first since they tend to be speard across fields and properties
            foreach (
                var evtInfo in
                    asmType.GetEvents(Constants.DefaultFlags))
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
                asmType.GetMembers(Constants.DefaultFlags);

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
                            var propMi = NfTypeName.GetMethodsForProperty(pi, asmType);
                            foreach(var pim in propMi)
                            {
                                cgm.opCodeCallsAndCallvirtsMetadatTokens.AddRange(Asm.GetCallsMetadataTokens(pim));
                            }
                        }
                        cgType.Properties.Add(cgm);
                    }
                }
                catch(Exception ex)
                {
                    Asm.AddLoaderExceptionToLog(null, ex);

                    if (!Settings.IgnoreReflectionMissingAsmError)
                        throw;

                    cgType.Properties.Add(new CgMember()
                    {
                        Name = mi.Name,
                        TypeName = "System.Object",
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

                    cgType.Fields.Add(new CgMember()
                    {
                        Name = mi.Name,
                        TypeName = "System.Object",
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
                            : string.Format("{0}.{1}", cgTypeName.Namespace, cgTypeName.ClassName));

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

                    cgType.Methods.Add(new CgMember()
                    {
                        Name = mi.Name,
                        TypeName = "System.Object",
                        SkipIt = true
                    });
                }
            }

            if (resolveDependencies)
            {
                ResolveAllMetadataTokens(cgType, assembly.ManifestModule);
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
            var gacAssemblies = Constants.UseReflectionOnlyLoad
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
                    var declaringTypeAsmName = declaringType == null ? null : declaringType.AssemblyQualifiedName;

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
                piType.NfBaseType().Name != "ValueType")
                return null;

            var cgMem = new CgMember
            {
                TypeName = Settings.LangStyle.TransformClrTypeSyntax(piType),
                IsEnumerableType = NfTypeName.IsEnumerableReturnType(piType),
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
            if (NfTypeName.IsEnumType(piType, out enumVals) && enumValueDictionary != null)
            {
                cgMem.IsEnum = true;
                var clearName = NfTypeName.GetLastTypeNameFromArrayAndGeneric(cgMem.TypeName, "<");
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

            if (NfTypeName.IsClrGeneratedType(fi.Name))
                return null;

            var fiType = fi.NfFieldType();

            if (valueTypeOnly && fiType.NfBaseType() != null && fiType.NfBaseType().Name != "ValueType")
                return null;

            var cgMem = new CgMember
            {
                TypeName = Settings.LangStyle.TransformClrTypeSyntax(fiType),
                IsEnumerableType = NfTypeName.IsEnumerableReturnType(fiType),
                Name = fi.Name,
                IsStatic = fi.IsStatic,
                MetadataToken = fi.MetadataToken
            };

            string[] enumVals;
            if (NfTypeName.IsEnumType(fiType, out enumVals) && enumValueDictionary != null)
            {
                cgMem.IsEnum = true;
                var clearName = NfTypeName.GetLastTypeNameFromArrayAndGeneric(cgMem.TypeName, "<");
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
                Name = Constants.CTOR_NAME,
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
            if (NfTypeName.IsClrMethodForProperty(mti.Name, out propName))
                return null;
            if (NfTypeName.IsClrGeneratedType(mti.Name))
                //these appear as '<SomeNameOfMethodAlreadyAdded>b__kifkj(...)'
                return null;

            var miReturnType = mti.NfReturnType();

            var cgMem = new CgMember
            {
                Name = mti.Name,
                TypeName = Settings.LangStyle.TransformClrTypeSyntax(miReturnType),
                IsStatic = mti.IsStatic,
                IsGeneric = mti.IsGenericMethod,
                IsEnumerableType = NfTypeName.IsEnumerableReturnType(miReturnType),
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
            if (!getCallvirtMetadataTokens) return cgMem;

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
    }
}

