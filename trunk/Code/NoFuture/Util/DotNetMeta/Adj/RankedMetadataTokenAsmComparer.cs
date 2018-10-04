using System;
using System.Collections.Generic;

namespace NoFuture.Util.DotNetMeta
{
    [Serializable]
    public class RankedMetadataTokenAsmComparer : IEqualityComparer<RankedMetadataTokenAsm>,
        IComparer<RankedMetadataTokenAsm>
    {
        public int Compare(RankedMetadataTokenAsm x, RankedMetadataTokenAsm y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            if (x.PageRank == y.PageRank)
            {
                if (x.HasPdb && !y.HasPdb)
                    return -1;
                if (!x.HasPdb && y.HasPdb)
                    return 1;
            }
            return x.PageRank > y.PageRank ? -1 : 1;
        }

        public bool Equals(RankedMetadataTokenAsm x, RankedMetadataTokenAsm y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(RankedMetadataTokenAsm obj)
        {
            return obj?.PageRank.GetHashCode() ?? 1;
        }
    }
}