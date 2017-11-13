using System;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// Additional data on a bank in the US Fed
    /// </summary>
    [Serializable]
    public class BankAssetsSummary : AssetsSummary
    {
        public double PercentForeignOwned { get; set; }
        public int DomesticBranches { get; set; }
        public int ForeignBranches { get; set; }
    }
}