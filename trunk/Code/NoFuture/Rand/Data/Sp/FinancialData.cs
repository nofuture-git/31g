using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class FinancialData
    {
        public NetConIncome Income { get; set; }
        public NetConAssets Assets { get; set; }
        public override string ToString()
        {
            var i = Income?.ToString() ?? "0";
            var w = Assets?.ToString() ?? "0";
            return $"{w} {i}";
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
    [Serializable]
    public class NetConIncome : ICited
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
    [Serializable]
    public class SummaryOfBusiness
    {
        public string PlainText { get; set; }
        public Util.Pos.ITagset[] TaggedText { get; set; }
    }
}
