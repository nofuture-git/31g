using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Tokens.DotNetMeta.TokenId;
using NoFuture.Tokens.DotNetMeta.TokenRank;
using NoFuture.Util.NfConsole;

namespace NoFuture.Tokens.DotNetMeta.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenPageRank : IaaProgramBaseCmd<TokenPageRankResponse>
    {
        public GetTokenPageRank(Program myProgram) : base(myProgram) { }
        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole();
            MyProgram.PrintToConsole($"{nameof(GetTokenPageRank)} invoked");
            MyProgram.ProgressMessageState = null;
            if (arg == null || !arg.Any())
            {
                MyProgram.PrintToConsole("no tokens were passed up on the socket");
                return JsonEncodedResponse(
                        new TokenPageRankResponse
                        {
                            Msg = "no tokens were passed up on the socket",
                            St = MetadataTokenStatus.Error
                        });
            }

            try
            {
                var json = Encoding.UTF8.GetString(arg);
                var allTokens = JsonConvert.DeserializeObject<TokenIdResponse>(json);
                if (allTokens?.Tokens == null || !allTokens.Tokens.Any())
                {
                    return JsonEncodedResponse(new TokenPageRankResponse
                    {
                        Msg = "parse from JSON failed",
                        St = MetadataTokenStatus.Error
                    });
                }

                var pageRank = ((IaaProgram) MyProgram).UtilityMethods.ResolveTokenPageRanks(allTokens);
                Console.Write('\n');

                return JsonEncodedResponse(new TokenPageRankResponse {Ranks = pageRank});

            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(new TokenPageRankResponse
                {
                    Msg = string.Format(ex.Message),
                    St = MetadataTokenStatus.Error
                });
            }
        }
    }
}
