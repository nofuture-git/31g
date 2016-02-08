using System;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Util;

namespace NoFuture.Rand.Domus.Sp //Sequere pecuniam
{
    public interface ILoan
    {
        Identifier Id { get; set; }
        string Description { get; set; }

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

    public interface IFixedRateLoan : ILoan
    {
        /// <summary>
        /// The interest rate for the loan 
        /// </summary>
        float Rate { get; set; }

        /// <summary>
        /// The rate used to calc the minimum payment.
        /// </summary>
        float MinPaymentRate { get; set; }
    }

    [Serializable]
    public class FixedRateLoan : IFixedRateLoan 
    {
        #region fields
        private readonly TradeLine _tradeLine;
        private float _minPaymentRate;
        #endregion

        #region ctors
        public FixedRateLoan(DateTime openedDate, float minPaymentRate)
        {
            _tradeLine = new TradeLine(openedDate);
            _minPaymentRate = minPaymentRate;
        }
        #endregion

        #region properties

        public float MinPaymentRate
        {
            get { return _minPaymentRate; }
            set { _minPaymentRate = value; }
        }

        public Identifier Id { get; set; }
        public string Description { get; set; }
        public TradeLine TradeLine { get { return _tradeLine; }}
        public float Rate { get; set; }
        public IFirm Lender { get; set; }
        #endregion

        #region methods
        public Pecuniam GetMinPayment(DateTime dt)
        {
            var bal = _tradeLine.Balance.GetCurrent(dt, Rate);
            if (bal < new Pecuniam(0))
                return new Pecuniam(0);

            var amt = bal.Amount * Convert.ToDecimal(MinPaymentRate);
            return new Pecuniam(Math.Round(amt, 2) * -1);
        }

        public LoanStatus GetStatus(DateTime dt)
        {
            if ((_tradeLine.Closure != null && DateTime.Compare(_tradeLine.Closure.Value.ClosedDate, dt) < 0))
                return LoanStatus.Closed;

            if (_tradeLine.Balance.Transactions.Count <= 0)
                return LoanStatus.NoHistory;

            //make sure something is actually owed
            if((_tradeLine.Balance.GetCurrent(dt, Rate)).Amount <= 0)
                return LoanStatus.Current;

            var lastPayment =
                _tradeLine.Balance.GetPaymentSum(
                    new Tuple<DateTime, DateTime>(dt.AddDays(_tradeLine.DueFrequency.TotalDays*-1), dt));

            return lastPayment.Abs < GetMinPayment(dt).Abs
                ? LoanStatus.Late
                : LoanStatus.Current;
        }

        public PastDue? GetDelinquency(DateTime dt)
        {
            if (GetStatus(dt) != LoanStatus.Late)
                return null;

            //30 days past due when last payment was normal billing cycle plus 30 days
            var billingCycleDays = TradeLine.DueFrequency.TotalDays;

            var justLate = new Tuple<DateTime, DateTime>(dt.AddDays(-29 - billingCycleDays),
                dt.AddDays(billingCycleDays*-1));

            if ((_tradeLine.Balance.GetPaymentSum(justLate)).Amount < 0)
                return null;

            //the line was openned some time before 30DPD
            if (DateTime.Compare(TradeLine.OpennedDate, dt.AddDays(-30 - billingCycleDays)) > 0)
                return null;

            var thirtyDpd = new Tuple<DateTime, DateTime>(dt.AddDays(-59 - billingCycleDays),
                dt.AddDays(-30 - billingCycleDays));

            if((_tradeLine.Balance.GetPaymentSum(thirtyDpd)).Amount < 0)
                return PastDue.Thirty;

            if (DateTime.Compare(TradeLine.OpennedDate, dt.AddDays(-60 - billingCycleDays)) > 1)
                return PastDue.Thirty;

            var sixtyDpd = new Tuple<DateTime, DateTime>(dt.AddDays(-89 - billingCycleDays),
                dt.AddDays(-60 - billingCycleDays));

            if ((TradeLine.Balance.GetPaymentSum(sixtyDpd)).Amount < 0)
                return PastDue.Sixty;

            if (DateTime.Compare(TradeLine.OpennedDate, dt.AddDays(-90 - billingCycleDays)) > 1)
                return PastDue.Sixty;

            var nintyDpd = new Tuple<DateTime, DateTime>(dt.AddDays(-179 - billingCycleDays),
                dt.AddDays(-90 - billingCycleDays));

            if((TradeLine.Balance.GetPaymentSum(nintyDpd)).Amount < 0)
                return PastDue.Ninety;

            if(DateTime.Compare(TradeLine.OpennedDate, dt.AddDays(-180 - billingCycleDays)) > 1)
                return PastDue.Ninety;

            return PastDue.HundredAndEighty;
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
