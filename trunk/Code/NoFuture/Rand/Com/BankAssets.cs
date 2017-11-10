using System;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Com
{
    [Serializable]
    public class BankAssets : SpAssets
    {
        public double PercentForeignOwned { get; set; }
        public int DomesticBranches { get; set; }
        public int ForeignBranches { get; set; }
    }
}