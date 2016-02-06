using System;

namespace NoFuture.Rand.Domus.Sp
{
    public interface ITradeLine
    {
        FormOfCredit FormOfCredit { get; set; }
        Pecuniam CreditLimit { get; set; }
        Balance CurrentBalance { get; }
        TimeSpan DueFrequency { get; set; }
        DateTime OpennedDate { get; }
        TradelineClosure? Closure { get; set; }
    }

    [Serializable]
    public class TradeLine : ITradeLine
    {
        #region constants
        public const int MAX_FICO = 850;
        public const int MIN_FICO = 300;
        #endregion

        #region fields
        private readonly Balance _balance = new Balance();
        private readonly DateTime _openDate;
        #endregion

        public TradeLine(DateTime openDate)
        {
            _openDate = openDate;
        }

        #region properties
        public FormOfCredit FormOfCredit { get; set; }
        public Pecuniam CreditLimit { get; set; }
        public Balance CurrentBalance { get { return _balance; } }
        public TimeSpan DueFrequency { get; set; }
        public DateTime OpennedDate { get { return _openDate; } }
        public TradelineClosure? Closure { get; set; }
        #endregion
    }

}
