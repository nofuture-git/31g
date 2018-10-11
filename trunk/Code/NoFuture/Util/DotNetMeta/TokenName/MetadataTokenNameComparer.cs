using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Util.DotNetMeta.TokenType;

namespace NoFuture.Util.DotNetMeta.TokenName
{
    [Serializable]
    public class MetadataTokenNameComparer : MetadataTokenTypeComparer
    {
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