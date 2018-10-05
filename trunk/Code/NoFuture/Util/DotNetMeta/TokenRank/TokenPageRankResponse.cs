using System;
using NoFuture.Util.DotNetMeta.Auxx;

namespace NoFuture.Util.DotNetMeta.Adj
{
    [Serializable]
    public class TokenPageRankResponse
    {
        public string Msg;
        public MetadataTokenStatus St;
        public Tuple<int, int, double>[] Ranks;
    }
}