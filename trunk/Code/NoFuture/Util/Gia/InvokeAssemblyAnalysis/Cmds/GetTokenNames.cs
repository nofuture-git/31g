using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenNames : CmdBase<TokenNames>
    {
        public override byte[] Execute(byte[] arg)
        {
            Program.PrintToConsole("GetTokenNames invoked");
            Program.ProgressMessageState = null;
            try
            {
                if (Program.AsmInited != true)
                {
                    Program.PrintToConsole("no assemblies are loaded - call GetAsmIndices");
                    return EncodedResponse(
                            new TokenNames
                            {
                                Msg = "no assemblies are loaded - call GetAsmIndices",
                                St = MetadataTokenStatus.Error
                            });

                }

                var tokens = JsonConvert.DeserializeObject<MetadataTokenId[]>(Encoding.UTF8.GetString(arg));
    
                if (tokens == null || tokens.Length <= 0)
                {
                    return EncodedResponse(new TokenNames
                    {
                        Msg = "parse failed",
                        St = MetadataTokenStatus.Error
                    });
                }

                var names = UtilityMethods.ResolveAllTokenNames(tokens);
                Console.Write('\n');

                return EncodedResponse(new TokenNames {Names = names.ToArray()});
            }
            catch (Exception ex)
            {
                Program.PrintToConsole(ex);
                return EncodedResponse(new TokenNames
                {
                    Msg = string.Format(ex.Message),
                    St = MetadataTokenStatus.Error
                });
            }
        }
    }
}
