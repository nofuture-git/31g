﻿using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared;
using NoFuture.Util.DotNetMeta;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenPageRank : CmdBase<TokenPageRanks>, ICmd
    {
        public GetTokenPageRank(Program myProgram) : base(myProgram) { }
        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole($"{nameof(GetTokenPageRank)} invoked");
            MyProgram.ProgressMessageState = null;
            if (arg == null || !arg.Any())
            {
                MyProgram.PrintToConsole("no tokens were passed up on the socket");
                return JsonEncodedResponse(
                        new TokenPageRanks
                        {
                            Msg = "no tokens were passed up on the socket",
                            St = MetadataTokenStatus.Error
                        });
            }

            try
            {
                var allTokens = JsonConvert.DeserializeObject<TokenIds>(Encoding.UTF8.GetString(arg));
                if (allTokens?.Tokens == null || !allTokens.Tokens.Any())
                {
                    return JsonEncodedResponse(new TokenPageRanks
                    {
                        Msg = "parse from JSON failed",
                        St = MetadataTokenStatus.Error
                    });
                }

                var pageRank = ((IaaProgram) MyProgram).UtilityMethods.ResolveTokenPageRanks(allTokens);
                Console.Write('\n');

                return JsonEncodedResponse(new TokenPageRanks {Ranks = pageRank});

            }
            catch (Exception ex)
            {
                MyProgram.PrintToConsole(ex);
                return JsonEncodedResponse(new TokenPageRanks
                {
                    Msg = string.Format(ex.Message),
                    St = MetadataTokenStatus.Error
                });
            }
        }
    }
}
