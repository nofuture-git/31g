using System;

namespace NoFuture.Shared
{
    [Serializable]
    public class RankedMetadataTokenAsm : MetadataTokenAsm
    {
        public double PageRank;
        public string DllFullName;
        public bool HasPdb;
    }
}