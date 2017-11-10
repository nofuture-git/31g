using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class SpAssets : ICited
    {
        public string Src { get; set; }
        public Pecuniam DomesticAssets { get; set; }
        public Pecuniam TotalAssets { get; set; }
        public Pecuniam TotalLiabilities { get; set; }

        public Pecuniam NetWorthMargin
        {
            get
            {
                if(TotalLiabilities == null || TotalLiabilities.Amount == 0)
                    return Pecuniam.Zero;
                var nw = TotalAssets/TotalLiabilities;
                return new Pecuniam(Math.Round(nw.Amount, 3));
            }
        }

        public override string ToString()
        {
            return (TotalAssets ?? Pecuniam.Zero).ToString();
        }
    }
}