using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenIds : CmdBase<TokenIds>
    {
        public override byte[] Execute(byte[] arg)
        {
            Program.PrintToConsole("GetTokenIds invoked");
            try
            {
                if (Program.AsmInited != true)
                {
                    Program.PrintToConsole("no assemblies are loaded - call GetAsmIndices");
                    return EncodedResponse(
                            new TokenIds
                            {
                                Msg = "no assemblies are loaded - call GetAsmIndices",
                                St = MetadataTokenStatus.Error
                            });

                }
                if (arg == null || arg.Length <= 0)
                {
                    Program.PrintToConsole("the arg for GetTokenIds is null or empty");
                    return EncodedResponse(
                            new TokenIds
                            {
                                Msg = "the arg for GetTokenIds is null or empty",
                                St = MetadataTokenStatus.Error
                            });
                } 
                
                var asmName =
                    JsonConvert.DeserializeObject<string>(Encoding.UTF8.GetString(arg));

                if (string.IsNullOrWhiteSpace(asmName))
                {
                    Program.PrintToConsole("could not deserialize the byte array");
                    return EncodedResponse(
                        new TokenIds
                        {
                            Msg = "could not deserialize the byte array",
                            St = MetadataTokenStatus.Error
                        });

                }

                Assembly asm;
                if (
                    Program.AsmIndicies.Asms.All(
                        x =>
                            string.Equals(x.AssemblyName, asmName,
                                StringComparison.OrdinalIgnoreCase)))
                {
                    asm = Program.Assembly;
                }
                else
                {
                    asm = AppDomain.CurrentDomain.GetAssemblies()
                        .FirstOrDefault(
                            x =>
                                string.Equals(x.GetName().FullName, asmName,
                                    StringComparison.OrdinalIgnoreCase));
                }

                if (asm == null)
                {
                    Program.PrintToConsole("could not locate a matching assembly");
                    return EncodedResponse(
                        new TokenIds
                        {
                            Msg = "could not locate a matching assembly",
                            St = MetadataTokenStatus.Error
                        });
                }

                var asmTypes = asm.GetTypes();
                var tokens = asmTypes.Select(Gia.AssemblyAnalysis.GetMetadataToken).ToList();

                return EncodedResponse(
                    new TokenIds
                    {
                        Tokens = tokens.ToArray()
                    });

            }
            catch (Exception ex)
            {
                Program.PrintToConsole(ex);
                return EncodedResponse(
                    new TokenIds
                    {
                        Msg = ex.Message,
                        St = MetadataTokenStatus.Error
                    });
            }
        }
    }
}
