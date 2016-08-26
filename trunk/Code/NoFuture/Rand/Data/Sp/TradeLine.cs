using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public enum ClosedCondition
    {
        ClosedWithZeroBalance,
        VoluntarySurrender,
        ClosureSurrender,
        Repossession,
        ChargeOff,
        Foreclosure
    }

    [Serializable]
    public struct TradelineClosure
    {
        public DateTime ClosedDate;
        public ClosedCondition Condition;
    }

    public interface ITradeLine
    {
        FormOfCredit FormOfCredit { get; set; }
        Pecuniam CreditLimit { get; set; }
        IBalance Balance { get; }
        TimeSpan DueFrequency { get; set; }
        DateTime OpennedDate { get; }
        TradelineClosure? Closure { get; set; }
    }

    [Serializable]
    public class TradeLine : Identifier, ITradeLine
    {
        #region constants
        public static TimeSpan DefaultDueFrequency = new TimeSpan(30,0,0,0);
        #endregion

        #region fields
        private readonly Guid _uniqueId = Guid.NewGuid();
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
        public IBalance Balance => _balance;
        public TimeSpan DueFrequency { get { return _dueFrequency; } set { _dueFrequency = value; }}
        public DateTime OpennedDate => _openDate;
        public TradelineClosure? Closure { get; set; }
        #endregion

        #region methods

        public override string Abbrev => "Tradeline";

        public override string ToString()
        {
            return _uniqueId.ToString();
        }

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
