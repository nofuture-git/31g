using System;
using System.Collections.Generic;

namespace NoFuture.Shared
{
    [Serializable]
    public class MetadataTokenNameComparer : IEqualityComparer<MetadataTokenName>
    {
        public bool Equals(MetadataTokenName x, MetadataTokenName y)
        {
            if (x == null || y == null)
                return false;
            return x.GetNameHashCode() == y.GetNameHashCode();
        }

        public int GetHashCode(MetadataTokenName obj)
        {
            return obj.GetNameHashCode();
        }
    }
}