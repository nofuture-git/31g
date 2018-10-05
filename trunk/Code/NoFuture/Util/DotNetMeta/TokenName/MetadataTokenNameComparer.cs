using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Util.DotNetMeta.TokenName
{
    [Serializable]
    public class MetadataTokenNameComparer : IEqualityComparer<MetadataTokenName>,
        IComparer<MetadataTokenName>
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

        public int Compare(MetadataTokenName x, MetadataTokenName y)
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

        public MetadataTokenName ChooseOne(IEnumerable<MetadataTokenName> choices)
        {
            if (choices == null || !choices.Any())
                return null;
            if (choices.Count() == 1)
                return choices.FirstOrDefault();

            var maxDepth = choices.Max(c => c.GetFullDepthCount());
            var choicesAtMaxDepth = choices.Where(c => c.GetFullDepthCount() == maxDepth);

            return choicesAtMaxDepth.FirstOrDefault(c => !c.IsByRef);
        }
    }
}