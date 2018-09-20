using System;

namespace NoFuture.Shared.DotNetMeta
{
    [Serializable]
    public class RankedMetadataTokenAsm : MetadataTokenAsm
    {
        public double PageRank;
        public string DllFullName;
        public bool HasPdb;
    }
}