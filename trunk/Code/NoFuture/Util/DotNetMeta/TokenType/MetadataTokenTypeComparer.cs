using System;
using System.Collections.Generic;

namespace NoFuture.Util.DotNetMeta.TokenType
{
    [Serializable]
    public class MetadataTokenTypeComparer : IEqualityComparer<INfToken>,
        IComparer<INfToken>
    {
        public bool Equals(INfToken x, INfToken y)
        {
            if (object.Equals(x, y))
                return true;
            if (string.IsNullOrWhiteSpace(x?.Name) || string.IsNullOrWhiteSpace(y?.Name))
                return false;
            return x.GetNameHashCode() == y.GetNameHashCode();
        }

        public int GetHashCode(INfToken obj)
        {
            return obj.GetNameHashCode();
        }

        public int Compare(INfToken x, INfToken y)
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