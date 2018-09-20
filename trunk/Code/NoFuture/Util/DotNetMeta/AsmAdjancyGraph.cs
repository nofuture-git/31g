using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Util.DotNetMeta
{
    [Serializable]
    public class AsmAdjancyGraph
    {
        public string Msg;
        public MetadataTokenStatus St;
        public RankedMetadataTokenAsm[] Asms;
        [NonSerialized]
        public int[,] Graph;

        public SortedSet<RankedMetadataTokenAsm> GetRankedAsms()
        {
            var ss = new SortedSet<RankedMetadataTokenAsm>(new RankedMetadataTokenAsmComparer());
            if (Asms == null || !Asms.Any())
                return ss;
            foreach (var a in Asms)
                ss.Add(a);

            return ss;
        }
    }
}