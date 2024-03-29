﻿using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared.Cfg;
using NoFuture.Tokens.DotNetMeta.TokenAsm;
using NoFuture.Tokens.DotNetMeta.TokenId;
using NoFuture.Util.NfConsole;

namespace NoFuture.Tokens.DotNetMeta.InvokeAssemblyAnalysis.Cmds
{
    public class GetAsmIndices : IaaProgramBaseCmd<AsmIndexResponse>
    {
        internal static string WAK_WAK = new string(new[] { (char)0x5C, (char)0x5C });
        public GetAsmIndices(Program myProgram)
            : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole();
            MyProgram.PrintToConsole($"{nameof(GetAsmIndices)} invoked");
            try
            {
                if (arg == null || arg.Length <= 0)
                {
                    MyProgram.PrintToConsole("the arg for GetAsmIndices is null or empty");
                    return JsonEncodedResponse(
                        new AsmIndexResponse
                        {
                            Msg = "the arg for GetAsmIndices is null or empty",
                            St = MetadataTokenStatus.Error
                        });
                }

                var json = Encoding.UTF8.GetString(arg);
                var rqst = JsonConvert.DeserializeObject<AsmIndexRequest>(json);
                var asmPath = rqst.AssemblyFilePath.Replace("\"",string.Empty);
                if (asmPath.Contains(WAK_WAK) && !asmPath.StartsWith(WAK_WAK))
                {
                    asmPath = asmPath.Replace(WAK_WAK, new string(new[] { (char)0x5C }));
                }

                if (!File.Exists(asmPath))
                {

                    MyProgram.PrintToConsole(string.Format("invalid path [{0}]", asmPath));
                    return JsonEncodedResponse(
                        new AsmIndexResponse
                        {
                            Msg = string.Format("invalid path [{0}]", asmPath),
                            St = MetadataTokenStatus.Error
                        });
                } 

                MyProgram.PrintToConsole($"Assembly: {Path.GetFileName(asmPath)}");
                ((IaaProgram) MyProgram).RootAssemblyPath = asmPath;

                NfConfig.AssemblySearchPaths.Add(Path.GetDirectoryName(asmPath));

                var asm = NfConfig.UseReflectionOnlyLoad
                    ? Util.Binary.Asm.NfReflectionOnlyLoadFrom(asmPath)
                    : Util.Binary.Asm.NfLoadFrom(asmPath);

                if (asm == null)
                {
                    MyProgram.PrintToConsole("cannot load assembly");
                    return JsonEncodedResponse(
                        new AsmIndexResponse
                        {
                            Msg = "cannot load assembly",
                            St = MetadataTokenStatus.Error
                        });
                }

                ((IaaProgram)MyProgram).Init(asm);
                ((IaaProgram)MyProgram).AssignAsmIndicies(asm);

                return JsonEncodedResponse(((IaaProgram)MyProgram).AsmIndicies);
            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(
                    new AsmIndexResponse
                    {
                        Msg = string.Format(ex.Message),
                        St = MetadataTokenStatus.Error
                    });
            }
        }
    }
}
