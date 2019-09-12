using System;
using NoFuture.Util.DotNetMeta.TokenAsm;

namespace NoFuture.Util.DotNetMeta.TokenRank
{
    [Serializable]
    public class RankedMetadataTokenAsm : MetadataTokenAsm
    {
        public double PageRank { get; set; }
        public string DllFullName { get; set; }
        public bool HasPdb { get; set; }
    }
}