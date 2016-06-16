﻿using System;
using System.IO;
using System.Text;
using NoFuture.Shared;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetAsmIndices : CmdBase<AsmIndicies>, ICmd
    {
        internal static string WAK_WAK = new string(new[] { (char)0x5C, (char)0x5C });
        public GetAsmIndices(Program myProgram)
            : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole("GetAsmIndices invoked");
            try
            {
                if (arg == null || arg.Length <= 0)
                {
                    MyProgram.PrintToConsole("the arg for GetAsmIndices is null or empty");
                    return EncodedResponse(
                        new AsmIndicies
                        {
                            Msg = "the arg for GetAsmIndices is null or empty",
                            St = MetadataTokenStatus.Error
                        });
                }
                var asmPath = Encoding.UTF8.GetString(arg).Replace("\"",string.Empty);
                if (asmPath.Contains(WAK_WAK) && !asmPath.StartsWith(WAK_WAK))
                {
                    asmPath = asmPath.Replace(WAK_WAK, new string(new[] { (char)0x5C }));
                }

                if (!File.Exists(asmPath))
                {

                    MyProgram.PrintToConsole(string.Format("invalid path [{0}]", asmPath));
                    return EncodedResponse(
                        new AsmIndicies
                        {
                            Msg = string.Format("invalid path [{0}]", asmPath),
                            St = MetadataTokenStatus.Error
                        });
                } 
                
                NfConfig.AssemblySearchPaths.Add(Path.GetDirectoryName(asmPath));

                var asm = NfConfig.UseReflectionOnlyLoad
                    ? Binary.Asm.NfReflectionOnlyLoadFrom(asmPath)
                    : Binary.Asm.NfLoadFrom(asmPath);

                if (asm == null)
                {
                    MyProgram.PrintToConsole("cannot load assembly");
                    return EncodedResponse(
                        new AsmIndicies
                        {
                            Msg = "cannot load assembly",
                            St = MetadataTokenStatus.Error
                        });
                }

                ((IaaProgram)MyProgram).Init(asm);
                ((IaaProgram)MyProgram).AssignAsmIndicies(asm);

                return EncodedResponse(((IaaProgram)MyProgram).AsmIndicies);
            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return EncodedResponse(
                    new AsmIndicies
                    {
                        Msg = string.Format(ex.Message),
                        St = MetadataTokenStatus.Error
                    });
            }
        }
    }
}
