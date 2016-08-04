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

    public interface IReceivable
    {
        Identifier Id { get; set; }
        string Description { get; set; }

        /// <summary>
        /// Determins the deliquency for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        PastDue? GetDelinquency(DateTime dt);

        /// <summary>
        /// Calc's the minimum payment for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pecuniam GetMinPayment(DateTime dt);

        /// <summary>
        /// Get the loans status for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        AccountStatus GetStatus(DateTime dt);

        Pecuniam GetCurrentBalance(DateTime dt);
    }

    public abstract class ReceivableBase : IReceivable
    {
        protected internal TradeLine _tl;
        protected ReceivableBase(DateTime openedDate)
        {
            _tl = new TradeLine(openedDate);
        }

        public virtual TradeLine TradeLine => _tl;
        public Identifier Id { get; set; }
        public string Description { get; set; }

        public virtual PastDue? GetDelinquency(DateTime dt)
        {
            if (GetStatus(dt) != AccountStatus.Late)
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

        public virtual AccountStatus GetStatus(DateTime dt)
        {
            if (TradeLine.Closure != null && DateTime.Compare(TradeLine.Closure.Value.ClosedDate, dt) < 0)
                return AccountStatus.Closed;

            if (TradeLine.Balance.Transactions.Count <= 0)
                return AccountStatus.NoHistory;

            //make sure something is actually owed
            if (GetCurrentBalance(dt).Amount <= 0)
                return AccountStatus.Current;

            var lastPayment =
                TradeLine.Balance.GetDebitSum(
                    new Tuple<DateTime, DateTime>(dt.AddDays(TradeLine.DueFrequency.TotalDays*-1), dt));

            return lastPayment.Abs < GetMinPayment(dt).Abs
                ? AccountStatus.Late
                : AccountStatus.Current;
        }

        public abstract Pecuniam GetCurrentBalance(DateTime dt);

        public abstract Pecuniam GetMinPayment(DateTime dt);

    }
}