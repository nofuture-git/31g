using System;
using System.Collections.Generic;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Data.Sp //Sequere pecuniam
{
    public interface ILoan
    {
        Identifier Id { get; set; }
        string Description { get; set; }

        /// <summary>
        /// Can be set to a combination of the <see cref="FormOfCredit"/>
        /// </summary>
        FormOfCredit KindOfLoan { get; set; }

        /// <summary>
        /// The rate used to calc the minimum payment.
        /// </summary>
        float MinPaymentRate { get; set; }

        /// <summary>
        /// Credit history report for this loan
        /// </summary>
        TradeLine TradeLine { get;}

        IFirm Lender { get; set; }

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
        LoanStatus GetStatus(DateTime dt);
    }

    [Serializable]
    public abstract class LoanBase<T> : ILoan
    {
        #region fields
        private readonly TradeLine _tradeLine;
        #endregion

        #region ctors
        protected LoanBase(DateTime openedDate, float minPaymentRate)
        {
            _tradeLine = new TradeLine(openedDate);
            MinPaymentRate = minPaymentRate;
        }
        #endregion

        #region properties
        public Identifier Id { get; set; }
        public string Description { get; set; }
        public FormOfCredit KindOfLoan { get; set; }
        public float MinPaymentRate { get; set; }
        public T Rate { get; set; }
        public TradeLine TradeLine { get { return _tradeLine; } }
        public IFirm Lender { get; set; }
        #endregion

        #region methods
        public virtual PastDue? GetDelinquency(DateTime dt)
        {
            if (GetStatus(dt) != LoanStatus.Late)
                return null;

            //30 days past due when last payment was normal billing cycle plus 30 days
            var billingCycleDays = TradeLine.DueFrequency.TotalDays;

            var justLate = new Tuple<DateTime, DateTime>(dt.AddDays(-29 - billingCycleDays),
                dt.AddDays(billingCycleDays * -1));

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

        public virtual Pecuniam GetMinPayment(DateTime dt)
        {
            var bal = GetCurrentBalance(dt, Rate);
            if (bal < new Pecuniam(0))
                return new Pecuniam(0);

            var amt = bal.Amount * Convert.ToDecimal(MinPaymentRate);
            return new Pecuniam(Math.Round(amt, 2) * -1);
        }

        public virtual LoanStatus GetStatus(DateTime dt)
        {
            if ((TradeLine.Closure != null && DateTime.Compare(TradeLine.Closure.Value.ClosedDate, dt) < 0))
                return LoanStatus.Closed;

            if (TradeLine.Balance.Transactions.Count <= 0)
                return LoanStatus.NoHistory;

            //make sure something is actually owed
            if ((GetCurrentBalance(dt, Rate)).Amount <= 0)
                return LoanStatus.Current;

            var lastPayment =
                TradeLine.Balance.GetDebitSum(
                    new Tuple<DateTime, DateTime>(dt.AddDays(TradeLine.DueFrequency.TotalDays * -1), dt));

            return lastPayment.Abs < GetMinPayment(dt).Abs
                ? LoanStatus.Late
                : LoanStatus.Current;
        }

        protected abstract Pecuniam GetCurrentBalance(DateTime dt, T rate);

        #endregion
    }

    [Serializable]
    public class FixedRateLoan : LoanBase<float>
    {
        #region ctors
        public FixedRateLoan(DateTime openedDate, float minPaymentRate): base(openedDate, minPaymentRate) { }
        #endregion

        #region methods
        protected override Pecuniam GetCurrentBalance(DateTime dt, float rate)
        {
            return TradeLine.Balance.GetCurrent(dt, Rate);
        }
        #endregion
    }

    [Serializable]
    public class VariableRateLoan : LoanBase<Dictionary<DateTime, float>>
    {
        #region ctors
        public VariableRateLoan(DateTime openedDate, float minPaymentRate) : base(openedDate, minPaymentRate) { }
        #endregion

        #region methods
        protected override Pecuniam GetCurrentBalance(DateTime dt, Dictionary<DateTime, float> rate)
        {
            return TradeLine.Balance.GetCurrent(dt, Rate);
        }
        #endregion
    }

    [Flags]
    [Serializable]
    public enum FormOfCredit : short
    {
        None = 0,
        Revolving = 1,
        Installment = 2,
        Mortgage = 4,
    }

    [Serializable]
    public enum LoanStatus
    {
        Closed,
        Current,
        Late,
        NoHistory
    }

    [Serializable]
    public enum PastDue
    {
        Thirty,
        Sixty,
        Ninety,
        HundredAndEighty
    }

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
}
