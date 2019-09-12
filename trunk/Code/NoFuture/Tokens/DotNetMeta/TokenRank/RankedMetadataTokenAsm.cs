using System;
using NoFuture.Tokens.DotNetMeta.TokenAsm;

namespace NoFuture.Tokens.DotNetMeta.TokenRank
{
    [Serializable]
    public class RankedMetadataTokenAsm : MetadataTokenAsm
    {
        public double PageRank { get; set; }
        public string DllFullName { get; set; }
        public bool HasPdb { get; set; }
    }
}