using System;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Com
{
    [Serializable]
    public class CommercialFinancialData : FinancialData
    {
        public int FiscalYear { get; set; }
        public int NumOfShares { get; set; }
    }
}