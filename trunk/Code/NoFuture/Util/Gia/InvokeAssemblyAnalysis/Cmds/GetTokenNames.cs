using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenNames : CmdBase<TokenNames>, ICmd
    {
        public GetTokenNames(Program myProgram) : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole("GetTokenNames invoked");
            MyProgram.ProgressMessageState = null;
            try
            {
                if (((IaaProgram)MyProgram).AsmInited != true)
                {
                    MyProgram.PrintToConsole("no assemblies are loaded - call GetAsmIndices");
                    return JsonEncodedResponse(
                            new TokenNames
                            {
                                Msg = "no assemblies are loaded - call GetAsmIndices",
                                St = MetadataTokenStatus.Error
                            });

                }

                var tokens = JsonConvert.DeserializeObject<MetadataTokenId[]>(Encoding.UTF8.GetString(arg));
    
                if (tokens == null || tokens.Length <= 0)
                {
                    return JsonEncodedResponse(new TokenNames
                    {
                        Msg = "parse failed",
                        St = MetadataTokenStatus.Error
                    });
                }

                var names = ((IaaProgram)MyProgram).UtilityMethods.ResolveAllTokenNames(tokens);
                Console.Write('\n');

                return JsonEncodedResponse(new TokenNames {Names = names.ToArray()});
            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(new TokenNames
                {
                    Msg = string.Format(ex.Message),
                    St = MetadataTokenStatus.Error
                });
            }
        }
    }
}
