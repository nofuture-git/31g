using System;
using NoFuture.Util.DotNetMeta.Xfer;

namespace NoFuture.Util.DotNetMeta.Adj
{
    [Serializable]
    public class RankedMetadataTokenAsm : MetadataTokenAsm
    {
        public double PageRank;
        public string DllFullName;
        public bool HasPdb;
    }
}