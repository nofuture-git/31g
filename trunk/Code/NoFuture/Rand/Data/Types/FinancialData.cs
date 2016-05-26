using System;

namespace NoFuture.Rand.Data.Types
{
    [Serializable]
    public class FinancialData
    {
        public int FiscalYear { get; set; }
        public Income Income { get; set; }
        public Assets Assets { get; set; }
        public int NumOfShares { get; set; }
    }
    [Serializable]
    public class Assets : ICited
    {
        public string Src { get; set; }
        public Pecuniam DomesticAssets { get; set; }
        public Pecuniam TotalAssets { get; set; }
        public Pecuniam TotalLiabilities { get; set; }
        public override string ToString()
        {
            return "{" + 
                   $"'DomesticAssets':{DomesticAssets}," +
                   $"'TotalAssets':{TotalAssets}," +
                   $"'TotalLiabilities':{TotalLiabilities}" + "}";
        }
    }
    [Serializable]
    public class Income : ICited
    {
        public string Src { get; set; }
        public Pecuniam Revenue { get; set; }
        public Pecuniam OperatingIncome { get; set; }
        public Pecuniam NetIncome { get; set; } //NI
        public override string ToString()
        {
            return "{" + 
                   $"'Revenue':{Revenue}," +
                   $"'OperatingIncome':{OperatingIncome}," +
                   $"'NetIncome':{NetIncome}" + "}";
        }
    }
    [Serializable]
    public class SummaryOfBusiness
    {
        public string PlainText { get; set; }
        public Util.Pos.ITagset[] TaggedText { get; set; }
    }
}
