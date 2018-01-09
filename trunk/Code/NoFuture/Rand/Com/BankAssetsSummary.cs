using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;

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

        public Pecuniam DomesticAssets { get; set; }
        public Pecuniam TotalAssets { get; set; }
        public Pecuniam TotalLiabilities { get; set; }

        public Pecuniam NetWorthMargin
        {
            get
            {
                if (TotalLiabilities == null || TotalLiabilities.Amount == 0)
                    return Pecuniam.Zero;
                var nw = TotalAssets / TotalLiabilities;
                return new Pecuniam(Math.Round(nw.Amount, 3));
            }
        }

        public override string ToString()
        {
            return (TotalAssets ?? Pecuniam.Zero).ToString();
        }
    }
}