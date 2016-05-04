using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NoFuture.Shared;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetAsmIndices : CmdBase<AsmIndicies>
    {
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
                var asmPath = Encoding.UTF8.GetString(arg);
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
                
                Constants.AssemblySearchPaths.Add(Path.GetDirectoryName(asmPath));

                var asm = Constants.UseReflectionOnlyLoad
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
