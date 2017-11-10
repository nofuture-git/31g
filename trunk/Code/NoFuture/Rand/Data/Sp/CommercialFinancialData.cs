using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class CommercialFinancialData : FinancialData
    {
        public int FiscalYear { get; set; }
        public int NumOfShares { get; set; }
    }
}