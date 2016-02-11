namespace NoFuture.Rand.Data.Types
{
    public class FinancialData
    {
        public string FiscalYearEndAt { get; set; }
        public Income Income { get; set; }
        public Assets Assets { get; set; }
        public int NumOfShares { get; set; }
    }

    public class Assets
    {
        public Pecuniam ConsolidatedAssets { get; set; }
        public Pecuniam DomesticAssets { get; set; }        
    }

    public class Income
    {
        public Pecuniam Revenue { get; set; }
        public Pecuniam OperatingCost { get; set; }
        public Pecuniam Tax { get; set; }
        public Pecuniam OperatingIncome
        {
            get { return new Pecuniam(Revenue.Amount - OperatingCost.Amount); }
        }

        public Pecuniam NetIncome
        {
            get { return new Pecuniam(OperatingIncome.Amount - Tax.Amount); }
        }
    }

    public class SummaryOfBusiness
    {
        public string PlainText { get; set; }
        public Util.Pos.ITagset[] TaggedText { get; set; }
    }
}
