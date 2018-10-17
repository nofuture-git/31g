using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.Binary;
using NoFuture.Util.Core;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenType;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenTypes : CmdBase<TokenTypeResponse>, ICmd
    {
        private AsmIndexResponse _asmIndices;
        private string _rootDir;
        public GetTokenTypes(Program myProgram) : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole();
            MyProgram.PrintToConsole($"{nameof(GetTokenTypes)} invoked");
            MyProgram.ProgressMessageState = null;
            try
            {
                //expect that the caller has init'ed this with some target assembly(ies)
                if (((IaaProgram)MyProgram).AsmInited != true)
                {
                    MyProgram.PrintToConsole("no assemblies are loaded - call GetAsmIndices");
                    return JsonEncodedResponse(
                        new TokenTypeResponse
                        {
                            Msg = "no assemblies are loaded - call GetAsmIndices",
                            St = MetadataTokenStatus.Error
                        });

                }

                var json = Encoding.UTF8.GetString(arg);
                var rqst = JsonConvert.DeserializeObject<TokenTypeRequest>(json);
                if(!string.IsNullOrWhiteSpace(rqst.ResolveAllNamedLike))
                    ((IaaProgram)MyProgram).AssemblyNameRegexPattern = rqst.ResolveAllNamedLike;

                if (!string.IsNullOrWhiteSpace(((IaaProgram) MyProgram).RootAssemblyPath))
                    _rootDir = System.IO.Path.GetDirectoryName(((IaaProgram) MyProgram).RootAssemblyPath);

                //get all the assemblies of this app domain
                _asmIndices = ((IaaProgram) MyProgram).AsmIndicies;

                MyProgram.PrintToConsole($"There are {_asmIndices.Asms.Length} assemblies in scope.");

                var allTypes = GetAllTypes();
                if (allTypes == null || !allTypes.Any())
                {
                    var msg = "Could not resolve any types from any of the assemblies.";
                    MyProgram.PrintToConsole(msg);
                    return JsonEncodedResponse(
                        new TokenTypeResponse
                        {
                            Msg = msg,
                            St = MetadataTokenStatus.Error
                        });
                }

                MyProgram.PrintToConsole();
                MyProgram.PrintToConsole($"There are {allTypes.Length} types in all assemblies.");
                var tokenTypes = new List<MetadataTokenType>();
                var totalTypes = allTypes.Length;
                for (var i = 0; i < totalTypes; i++)
                {
                    var cType = allTypes[i];

                    ((IaaProgram)MyProgram).ReportProgress(new ProgressMessage
                    {
                        Activity = $"{cType}",
                        ProcName = Process.GetCurrentProcess().ProcessName,
                        ProgressCounter = Etc.CalcProgressCounter(i, totalTypes),
                        Status = "Resolving all type names"
                    });

                    var tt = GetMetadataTokenType(cType);
                    if(tt != null)
                        tokenTypes.Add(tt);
                }

                var tokenTypeRspn = new TokenTypeResponse {Types = tokenTypes.ToArray()};
                //keep a copy of this in memory like AsmIndices
                ((IaaProgram) MyProgram).TokenTypeResponse = tokenTypeRspn;
                return JsonEncodedResponse(tokenTypeRspn);

            }
            catch (Exception ex)
            {
                Console.Write('\n');
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(
                    new TokenTypeResponse
                    {
                        Msg = ex.Message,
                        St = MetadataTokenStatus.Error
                    });
            }
        }

        public Type[] GetAllTypes()
        {
            var allTypes = new List<Type>();
            if (_asmIndices == null)
                return allTypes.ToArray();
            var totalAssemblies = _asmIndices.Asms.Length;
            for (var i = 0; i < totalAssemblies; i++)
            {
                var asmIdx = _asmIndices.Asms[i];
                ((IaaProgram)MyProgram).ReportProgress(new ProgressMessage
                {
                    Activity = $"{asmIdx.AssemblyName}",
                    ProcName = Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalAssemblies),
                    Status = "Resolving types per assembly"
                });

                var asm = _asmIndices.GetAssemblyByIndex(asmIdx.IndexId) ??
                          GetAssemblyFromFile(asmIdx.AssemblyName);
                if (asm == null)
                {
                    continue;
                }
                var ts = asm.NfGetTypes(false, MyProgram.LogFile);
                if (ts != null && ts.Any())
                    allTypes.AddRange(ts);
            }

            return allTypes.ToArray();
        }

        internal MetadataTokenType GetMetadataTokenType(Type cType)
        {
            if (string.IsNullOrWhiteSpace(cType?.FullName))
                return null;
            if (!IsMatch(cType))
                return null;

            var ownIdx = _asmIndices.GetAssemblyIndexByName(cType.Assembly.FullName) ?? 0;

            var tt = new MetadataTokenType
            {
                Id = cType.MetadataToken,
                Name = cType.FullName,
                OwnAsmIdx = ownIdx,
                IsIntfc = cType.IsInterface ? 1 : 0,
                IsAbsct = cType.IsAbstract ? 1 : 0
            };
            var ttInfcs = new List<MetadataTokenType>();
            var bType = cType.NfBaseType(false, MyProgram.LogFile);
            if (bType != null)
            {
                var ttB = GetMetadataTokenType(bType);
                if (ttB != null)
                    ttInfcs.Add(ttB);
            }

            foreach (var ifc in cType.GetInterfaces())
            {
                var ttInfc = GetMetadataTokenType(ifc);
                if(ttInfc != null)
                    ttInfcs.Add(ttInfc);
            }

            tt.Items = ttInfcs.ToArray();

            if (!cType.IsInterface && !cType.IsAbstract)
                return tt;

            var abstractMethods = cType.NfGetMembers(NfSettings.DefaultFlags, false, MyProgram.LogFile)
                .Where(mi => (mi as MethodInfo)?.IsAbstract ?? false);
            if (abstractMethods.Any())
                tt.AbstractMemberNames = abstractMethods.Select(mi =>
                        AssemblyAnalysis.ConvertToMetadataTokenName(mi, _asmIndices, null, MyProgram.LogFile, false))
                    .ToArray();

            return tt;
        }

        internal bool IsMatch(Type t)
        {
            var regexPattern = ((IaaProgram) MyProgram).AssemblyNameRegexPattern;
            if (string.IsNullOrWhiteSpace(regexPattern))
                return true;

            return t?.FullName != null &&
                        Regex.IsMatch(t.FullName, regexPattern);
        }

        internal Assembly GetAssemblyFromFile(string asmName)
        {
            //try it again on the drive
            if (string.IsNullOrWhiteSpace(_rootDir))
                return null;
            var di = new DirectoryInfo(_rootDir);
            foreach (var d in di.EnumerateFileSystemInfos())
            {
                if (!new[] { ".dll", ".exe" }.Contains(d.Extension))
                    continue;
                var dAsmName = Asm.GetAssemblyName(d.FullName, false, MyProgram.LogFile);
                var eAsmName = new AssemblyName(asmName);
                if (!AssemblyName.ReferenceMatchesDefinition(dAsmName, eAsmName))
                    continue;
                var asm = NfConfig.UseReflectionOnlyLoad
                    ? Asm.NfReflectionOnlyLoadFrom(d.FullName, false, MyProgram.LogFile)
                    : Asm.NfLoadFrom(d.FullName, false, MyProgram.LogFile);
                if (asm != null)
                    return asm;
            }

            return null;
        }
    }
}
