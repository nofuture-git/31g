using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NoFuture.Shared;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetAsmIndices : CmdBase<AsmIndicies>
    {
        public override byte[] Execute(byte[] arg)
        {
            Program.PrintToConsole("GetAsmIndices invoked");
            try
            {
                if (arg == null || arg.Length <= 0)
                {
                    Program.PrintToConsole("the arg for GetAsmIndices is null or empty");
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

                    Program.PrintToConsole(string.Format("invalid path [{0}]", asmPath));
                    return EncodedResponse(
                        new AsmIndicies
                        {
                            Msg = string.Format("invalid path [{0}]", asmPath),
                            St = MetadataTokenStatus.Error
                        });
                } 
                
                Constants.AssemblySearchPath = Path.GetDirectoryName(asmPath);

                var asm = Constants.UseReflectionOnlyLoad
                    ? Binary.Asm.NfReflectionOnlyLoadFrom(asmPath)
                    : Binary.Asm.NfLoadFrom(asmPath);

                if (asm == null)
                {
                    Program.PrintToConsole("cannot load assembly");
                    return EncodedResponse(
                        new AsmIndicies
                        {
                            Msg = "cannot load assembly",
                            St = MetadataTokenStatus.Error
                        });
                }

                Init(asm);
                AssignAsmIndicies(asm);

                return EncodedResponse(Program.AsmIndicies);
            }
            catch (Exception ex)
            {
                Program.PrintToConsole(ex);
                return EncodedResponse(
                    new AsmIndicies
                    {
                        Msg = string.Format(ex.Message),
                        St = MetadataTokenStatus.Error
                    });
            }
        }

        internal static void Init(Assembly asm)
        {
            Program.RootAssembly = asm;
            Program.TokenId2NameCache.Clear();
            Program.DisolutionCache.Clear();
            Program.DisolutionCache.Add(new MetadataTokenId());//empty token Id
        }

        internal static void AssignAsmIndicies(Assembly asm)
        {
            var tempList = new List<MetadataTokenAsm>();

            var refedAsms = asm.GetReferencedAssemblies();
            tempList.Add(new MetadataTokenAsm { AssemblyName = asm.GetName().FullName, IndexId = 0 });
            for (var i = 0; i < refedAsms.Length; i++)
            {
                tempList.Add(new MetadataTokenAsm { AssemblyName = refedAsms[i].FullName, IndexId = i + 1 });
            }

            Program.AsmIndicies.Asms = tempList.ToArray();
            Program.AsmIndicies.St = MetadataTokenStatus.Ok;            
        }
    }
}
