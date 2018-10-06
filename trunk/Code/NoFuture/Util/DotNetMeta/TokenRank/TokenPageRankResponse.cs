using System;
using NoFuture.Util.DotNetMeta.TokenId;

namespace NoFuture.Util.DotNetMeta.TokenRank
{
    [Serializable]
    public class TokenPageRankResponse
    {
        public string Msg { get; set; }
        public MetadataTokenStatus St { get; set; }
        public Tuple<int, int, double>[] Ranks { get; set; }
    }
}