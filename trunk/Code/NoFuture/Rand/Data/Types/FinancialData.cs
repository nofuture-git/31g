namespace NoFuture.Rand.Data.Types
{
    public class FinancialData
    {
        public string FiscalYearEndAt { get; set; }
        public int Income { get; set; }
        public int Assets { get; set; }
        public int Liabilities { get; set; }
        public int NumOfShares { get; set; }
    }

    public class SummaryOfBusiness
    {
        public string PlainText { get; set; }
        public Util.Pos.ITagset[] TaggedText { get; set; }
    }
}
