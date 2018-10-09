using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared;
using NoFuture.Util.DotNetMeta;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenNames : CmdBase<TokenNameResponse>, ICmd
    {
        public GetTokenNames(Program myProgram) : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole($"{nameof(GetTokenNames)} invoked");
            MyProgram.ProgressMessageState = null;
            try
            {
                if (((IaaProgram)MyProgram).AsmInited != true)
                {
                    MyProgram.PrintToConsole("no assemblies are loaded - call GetAsmIndices");
                    return JsonEncodedResponse(
                            new TokenNameResponse
                            {
                                Msg = "no assemblies are loaded - call GetAsmIndices",
                                St = MetadataTokenStatus.Error
                            });

                }

                var rqst = JsonConvert.DeserializeObject<TokenNameRequest>(Encoding.UTF8.GetString(arg));
                var tokens = rqst.Tokens;
    
                if (tokens == null || tokens.Length <= 0)
                {
                    return JsonEncodedResponse(new TokenNameResponse
                    {
                        Msg = "parse failed",
                        St = MetadataTokenStatus.Error
                    });
                }

                var names = ((IaaProgram)MyProgram).UtilityMethods.ResolveAllTokenNames(tokens);
                Console.Write('\n');

                return JsonEncodedResponse(new TokenNameResponse {Names = names.ToArray()});
            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(new TokenNameResponse
                {
                    Msg = string.Format(ex.Message),
                    St = MetadataTokenStatus.Error
                });
            }
        }
    }
}
