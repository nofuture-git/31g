using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// An aggregate type to represent a summary of income in terms of <see cref="Pecuniam"/>
    /// </summary>
    [Serializable]
    public class IncomeSummary : ICited
    {
        public string Src { get; set; }
        public Pecuniam Revenue { get; set; }
        public Pecuniam OperatingIncome { get; set; }
        public Pecuniam NetIncome { get; set; }

        public Pecuniam ProfitMargin
        {
            get
            {
                if(Revenue == null || Revenue.Amount == 0)
                    return Pecuniam.Zero;
                var pm = NetIncome/Revenue;

                return new Pecuniam(Math.Round(pm.Amount, 3));
            }
        }

        public override string ToString()
        {
            return (NetIncome ?? Pecuniam.Zero).ToString();
        }
    }
}