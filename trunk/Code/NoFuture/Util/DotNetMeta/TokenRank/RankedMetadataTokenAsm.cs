using System;
using NoFuture.Util.DotNetMeta.TokenAsm;

namespace NoFuture.Util.DotNetMeta.TokenRank
{
    [Serializable]
    public class RankedMetadataTokenAsm : MetadataTokenAsm
    {
        public double PageRank;
        public string DllFullName;
        public bool HasPdb;
    }
}