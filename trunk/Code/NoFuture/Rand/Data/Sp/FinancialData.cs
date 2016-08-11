using System;

namespace NoFuture.Rand.Data.Sp
{
    public interface IAsset
    {
        Pecuniam Value { get; }
        /// <summary>
        /// Get the loans status for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        AccountStatus GetStatus(DateTime dt);
    }

    [Serializable]
    public class FinancialData
    {
        public NetConIncome Income { get; set; }
        public NetConAssets Assets { get; set; }
        public override string ToString()
        {
            var i = Income?.ToString() ?? "0";
            var w = Assets?.ToString() ?? "0";
            return $"{i} {w}";
        }
    }

    [Serializable]
    public class ComFinancialData : FinancialData
    {
        public int FiscalYear { get; set; }
        public int NumOfShares { get; set; }
    }
    [Serializable]
    public class NetConAssets : ICited
    {
        public string Src { get; set; }
        public Pecuniam DomesticAssets { get; set; }
        public Pecuniam TotalAssets { get; set; }
        public Pecuniam TotalLiabilities { get; set; }

        public Pecuniam GetNetWorthMargin
        {
            get
            {
                if(TotalLiabilities.Amount == 0)
                    return Pecuniam.Zero;
                var nw = TotalAssets/TotalLiabilities;
                return new Pecuniam(Math.Round(nw.Amount, 3));
            }
        }

        public override string ToString()
        {
            return GetNetWorthMargin.ToString();
        }
    }
    [Serializable]
    public class NetConIncome : ICited
    {
        public string Src { get; set; }
        public Pecuniam Revenue { get; set; }
        public Pecuniam OperatingIncome { get; set; }
        public Pecuniam NetIncome { get; set; }

        public Pecuniam GetProfitMargin
        {
            get
            {
                if(Revenue.Amount == 0)
                    return Pecuniam.Zero;
                var pm = NetIncome/Revenue;

                return new Pecuniam(Math.Round(pm.Amount, 3));
            }
        }

        public override string ToString()
        {
            return GetProfitMargin.ToString();
        }
    }
    [Serializable]
    public class SummaryOfBusiness
    {
        public string PlainText { get; set; }
        public Util.Pos.ITagset[] TaggedText { get; set; }
    }
}
