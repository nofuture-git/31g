using System;
using NoFuture.Util.DotNetMeta.TokenId;

namespace NoFuture.Util.DotNetMeta.TokenRank
{
    [Serializable]
    public class TokenPageRankResponse
    {
        public string Msg;
        public MetadataTokenStatus St;
        public Tuple<int, int, double>[] Ranks;
    }
}