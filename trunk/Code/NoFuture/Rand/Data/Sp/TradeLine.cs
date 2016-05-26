using System;

namespace NoFuture.Rand.Domus.Sp
{
    public interface ITradeLine
    {
        FormOfCredit FormOfCredit { get; set; }
        Pecuniam CreditLimit { get; set; }
        Balance Balance { get; }
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

        public static TimeSpan DefaultDueFrequency = new TimeSpan(30,0,0,0);
        #endregion

        #region fields
        internal readonly Guid _uniqueId = Guid.NewGuid();
        private readonly Balance _balance = new Balance();
        private readonly DateTime _openDate;
        private TimeSpan _dueFrequency = DefaultDueFrequency;
        #endregion

        public TradeLine(DateTime openDate)
        {
            _openDate = openDate;
        }

        #region properties
        public FormOfCredit FormOfCredit { get; set; }
        public Pecuniam CreditLimit { get; set; }
        public Balance Balance { get { return _balance; } }
        public TimeSpan DueFrequency { get { return _dueFrequency; } set { _dueFrequency = value; }}
        public DateTime OpennedDate { get { return _openDate; } }
        public TradelineClosure? Closure { get; set; }
        #endregion

        #region methods

        public override bool Equals(object obj)
        {
            var tl = obj as TradeLine;
            if (tl == null)
                return false;
            return tl._uniqueId == _uniqueId;
        }

        public override int GetHashCode()
        {
            return _uniqueId.GetHashCode();
        }

        #endregion
    }

}
