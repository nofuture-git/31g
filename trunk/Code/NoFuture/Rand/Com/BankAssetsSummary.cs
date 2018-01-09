using System;
using System.Globalization;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Com
{
    /// <summary>
    /// Additional data on a bank in the US Fed
    /// </summary>
    [Serializable]
    public class BankAssetsSummary : ICited
    {
        public string Src { get; set; }
        public double PercentForeignOwned { get; set; }
        public int DomesticBranches { get; set; }
        public int ForeignBranches { get; set; }

        public decimal DomesticAssets { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal TotalLiabilities { get; set; }

        public decimal NetWorthMargin
        {
            get
            {
                var nw = TotalAssets / TotalLiabilities;
                return Math.Round(nw, 3);
            }
        }

        public override string ToString()
        {
            return TotalAssets.ToString(CultureInfo.InvariantCulture);
        }
    }
}