using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public enum PastDue
    {
        Thirty,
        Sixty,
        Ninety,
        HundredAndEighty
    }

    [Serializable]
    public abstract class ReceivableBase : IReceivable
    {
        #region fields
        protected internal TradeLine _tl;
        #endregion

        #region ctor
        protected ReceivableBase(DateTime openedDate)
        {
            _tl = new TradeLine(openedDate);
        }
        #endregion

        #region properties
        public virtual ITradeLine TradeLine => _tl;
        public string Description { get; set; }
        public SpStatus CurrentStatus => GetStatus(DateTime.Now);
        public PastDue? CurrentDelinquency => GetDelinquency(DateTime.Now);
        public Pecuniam CurrentMarketValue => GetBalance(DateTime.Now);
        #endregion

        #region methods
        public virtual PastDue? GetDelinquency(DateTime dt)
        {
            if (GetStatus(dt) != SpStatus.Late)
                return null;

            //30 days past due when last payment was normal billing cycle plus 30 days
            var billingCycleDays = TradeLine.DueFrequency.TotalDays;

            var justLate = new Tuple<DateTime, DateTime>(dt.AddDays(-29 - billingCycleDays),
                dt.AddDays(billingCycleDays*-1));

            if ((TradeLine.Balance.GetDebitSum(justLate)).Amount < 0)
                return null;

            //the line was openned some time before 30DPD
            if (DateTime.Compare(TradeLine.OpennedDate, dt.AddDays(-30 - billingCycleDays)) > 0)
                return null;

            var thirtyDpd = new Tuple<DateTime, DateTime>(dt.AddDays(-59 - billingCycleDays),
                dt.AddDays(-30 - billingCycleDays));

            if ((TradeLine.Balance.GetDebitSum(thirtyDpd)).Amount < 0)
                return PastDue.Thirty;

            if (DateTime.Compare(TradeLine.OpennedDate, dt.AddDays(-60 - billingCycleDays)) > 1)
                return PastDue.Thirty;

            var sixtyDpd = new Tuple<DateTime, DateTime>(dt.AddDays(-89 - billingCycleDays),
                dt.AddDays(-60 - billingCycleDays));

            if ((TradeLine.Balance.GetDebitSum(sixtyDpd)).Amount < 0)
                return PastDue.Sixty;

            if (DateTime.Compare(TradeLine.OpennedDate, dt.AddDays(-90 - billingCycleDays)) > 1)
                return PastDue.Sixty;

            var nintyDpd = new Tuple<DateTime, DateTime>(dt.AddDays(-179 - billingCycleDays),
                dt.AddDays(-90 - billingCycleDays));

            if ((TradeLine.Balance.GetDebitSum(nintyDpd)).Amount < 0)
                return PastDue.Ninety;

            if (DateTime.Compare(TradeLine.OpennedDate, dt.AddDays(-180 - billingCycleDays)) > 1)
                return PastDue.Ninety;

            return PastDue.HundredAndEighty;
        }

        public virtual SpStatus GetStatus(DateTime? dt)
        {
            var ddt = dt ?? DateTime.Now;
            if (TradeLine.Closure != null && DateTime.Compare(TradeLine.Closure.Value.ClosedDate, ddt) < 0)
                return SpStatus.Closed;

            if (TradeLine.Balance.IsEmpty)
                return SpStatus.NoHistory;

            //make sure something is actually owed
            if (GetBalance(ddt).Amount <= 0)
                return SpStatus.Current;

            var lastPayment =
                TradeLine.Balance.GetDebitSum(
                    new Tuple<DateTime, DateTime>(ddt.AddDays(TradeLine.DueFrequency.TotalDays*-1), ddt));

            return lastPayment.Abs < GetMinPayment(ddt).Abs
                ? SpStatus.Late
                : SpStatus.Current;
        }

        public abstract Pecuniam GetBalance(DateTime dt);

        public abstract Pecuniam GetMinPayment(DateTime dt);

        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Description) ? Description : base.ToString();
        }

        #endregion  
    }
}