using System;

namespace NoFuture.Shared.DotNetMeta
{
    [Serializable]
    public class TokenPageRanks
    {
        public string Msg;
        public MetadataTokenStatus St;
        public Tuple<int, int, double>[] Ranks;
    }
}