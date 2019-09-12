using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Tokens.DotNetMeta.TokenId;

namespace NoFuture.Tokens.DotNetMeta.TokenRank
{
    [Serializable]
    public class AsmAdjancyGraph : IEnumerable<RankedMetadataTokenAsm>
    {
        [NonSerialized]
        private int[,] _graph;

        public string Msg { get; set; }
        public MetadataTokenStatus St { get; set; }
        public RankedMetadataTokenAsm[] Asms { get; set; }

        public int[,] Graph
        {
            get => _graph;
            set => _graph = value;
        }

        public SortedSet<RankedMetadataTokenAsm> GetRankedAsms()
        {
            var ss = new SortedSet<RankedMetadataTokenAsm>(new RankedMetadataTokenAsmComparer());
            if (Asms == null || !Asms.Any())
                return ss;
            foreach (var a in Asms)
                ss.Add(a);

            return ss;
        }

        public IEnumerator<RankedMetadataTokenAsm> GetEnumerator()
        {
            return Asms.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count()
        {
            return Asms.Length;
        }
    }
}