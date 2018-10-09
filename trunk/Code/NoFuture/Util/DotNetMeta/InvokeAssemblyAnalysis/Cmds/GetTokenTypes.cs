using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.Binary;
using NoFuture.Util.Core;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenType;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenTypes : CmdBase<TokenTypeResponse>, ICmd
    {
        private AsmIndexResponse _asmIndices;
        public GetTokenTypes(Program myProgram) : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole($"{nameof(GetTokenIds)} invoked");
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
                var rqst = JsonConvert.DeserializeObject<TokenTypeRequest>(Encoding.UTF8.GetString(arg));
                if(!string.IsNullOrWhiteSpace(rqst.ResolveAllNamedLike))
                    ((IaaProgram)MyProgram).AssemblyNameRegexPattern = rqst.ResolveAllNamedLike;

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
                MyProgram.PrintToConsole($"There are {allTypes.Length} types in the all assemblies.");
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
                        Status = "Resolving names"
                    });

                    var tt = GetMetadataTokenType(cType);
                    if(tt != null)
                        tokenTypes.Add(tt);
                }
                return JsonEncodedResponse(new TokenTypeResponse {Types = tokenTypes.ToArray()});

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

            foreach (var asmIdx in _asmIndices.Asms)
            {
                if(asmIdx == null)
                    continue;

                var asm = _asmIndices.GetAssemblyByIndex(asmIdx.IndexId);
                if (asm == null)
                {
                    continue;
                }
                var ts = asm.NfGetTypes(false, MyProgram.LogFile);
                if(ts != null && ts.Any())
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
            var bType = cType.BaseType;
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
    }
}
