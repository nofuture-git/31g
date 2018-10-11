using System;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.Cmds
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
                var myIaaProgram = ((IaaProgram) MyProgram);
                if (myIaaProgram.AsmInited != true)
                {
                    MyProgram.PrintToConsole("no assemblies are loaded - call GetAsmIndices");
                    return JsonEncodedResponse(
                            new TokenNameResponse
                            {
                                Msg = "no assemblies are loaded - call GetAsmIndices",
                                St = MetadataTokenStatus.Error
                            });

                }

                var json = Encoding.UTF8.GetString(arg);
                var rqst = JsonConvert.DeserializeObject<TokenNameRequest>(json);
                var tokens = rqst.Tokens ?? myIaaProgram.TokenIdResponse?.Tokens;
    
                if (tokens == null || tokens.Length <= 0)
                {
                    return JsonEncodedResponse(new TokenNameResponse
                    {
                        Msg = "parse failed",
                        St = MetadataTokenStatus.Error
                    });
                }

                var names = myIaaProgram.UtilityMethods.ResolveAllTokenNames(tokens);
                Console.Write('\n');
                var tokenNameRspn = new TokenNameResponse {Names = names.ToArray()};
                if (rqst.MapFullCallStack && myIaaProgram.TokenTypeResponse != null
                    && myIaaProgram.TokenIdResponse != null)
                {
                    var nameRoot = tokenNameRspn.GetAsRoot();
                    var tokenRoot = myIaaProgram.TokenIdResponse.GetAsRoot();
                    var typeRoot = myIaaProgram.TokenTypeResponse.GetAsRoot();
                    var asmRspn = myIaaProgram.AsmIndicies;
                    var fullCallStack =
                        MetadataTokenName.BuildMetadataTokenName(nameRoot, tokenRoot, asmRspn, typeRoot, MyProgram.PrintToConsole);
                    if(fullCallStack?.Items != null)
                        tokenNameRspn.Names = fullCallStack.Items;
                }

                myIaaProgram.TokenNameResponse = tokenNameRspn;
                return JsonEncodedResponse(tokenNameRspn);
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
