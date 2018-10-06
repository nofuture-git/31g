using System;
using System.Collections.Generic;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    [Serializable]
    public class MetadataTokenTypeComparer : IEqualityComparer<MetadataTokenType>,
        IComparer<MetadataTokenType>
    {
        public bool Equals(MetadataTokenType x, MetadataTokenType y)
        {
            if (x == null || y == null)
                return false;
            return x.GetNameHashCode() == y.GetNameHashCode();
        }

        public int GetHashCode(MetadataTokenType obj)
        {
            return obj.GetNameHashCode();
        }

        public int Compare(MetadataTokenType x, MetadataTokenType y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            var xDepth = x.GetFullDepthCount();
            var yDepth = y.GetFullDepthCount();

            if (xDepth == yDepth)
            {
                return 0;
            }

            return xDepth > yDepth ? -1 : 1;
        }

    }
}