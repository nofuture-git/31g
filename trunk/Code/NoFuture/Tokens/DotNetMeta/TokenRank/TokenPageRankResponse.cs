using System;
using NoFuture.Tokens.DotNetMeta.TokenId;

namespace NoFuture.Tokens.DotNetMeta.TokenRank
{
    [Serializable]
    public class TokenPageRankResponse
    {
        public string Msg { get; set; }
        public MetadataTokenStatus St { get; set; }
        public Tuple<int, int, double>[] Ranks { get; set; }
    }
}