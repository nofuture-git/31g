using System;

namespace NoFuture.Rand.Domus.Sp
{
    [Serializable]
    public class TradeLine
    {
        public FormOfCredit FormOfCredit { get; set; }
        public Pecuniam CreditLimit { get; set; }
        public Pecuniam CurrentBalance { get; set; }
        public TimeSpan DueFrequency { get; set; }
        public LoanStatus Status { get; set; }
        public PastDue? Delinquency { get; set; }
        public DateTime OpennedDate { get; set; }
        public TradelineClosure? Closure { get; set; }
    }
}
