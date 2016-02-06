using System;
using System.Linq;
using NoFuture.Rand.Com;

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
            var bal = _tradeLine.CurrentBalance.GetCurrent(dt, Rate);
            if (bal < new Pecuniam(0))
                return new Pecuniam(0);

            var amt = bal.Amount * Convert.ToDecimal(MinPaymentRate);
            return new Pecuniam(Math.Round(amt, 0));
        }

        public LoanStatus GetStatus(DateTime dt)
        {
            if ((_tradeLine.Closure != null && DateTime.Compare(_tradeLine.Closure.Value.ClosedDate, dt) < 0))
                return LoanStatus.Closed;

            if (_tradeLine.CurrentBalance.Transactions.Count <= 0)
                return LoanStatus.NoHistory;

            var lastPayment =
                _tradeLine.CurrentBalance.GetPaymentSum(
                    new Tuple<DateTime, DateTime>(dt.AddDays(_tradeLine.DueFrequency.TotalDays*-1), dt));

            return lastPayment < GetMinPayment(dt)
                ? LoanStatus.Late
                : LoanStatus.Current;
        }

        public PastDue? GetDelinquency(DateTime dt)
        {
            if (GetStatus(dt) != LoanStatus.Late)
                return null;

            var lastPayment = _tradeLine.CurrentBalance.Transactions.LastOrDefault();
            if (lastPayment == null)
                return null;

            var daysFromLastPayment = (dt - lastPayment.AtTime).TotalDays;

            if (daysFromLastPayment >= 30 && daysFromLastPayment < 60)
                return PastDue.Thirty;
            if (daysFromLastPayment >= 60 && daysFromLastPayment < 90)
                return PastDue.Thirty;
            if (daysFromLastPayment >= 90 && daysFromLastPayment < 180)
                return PastDue.Ninety;
            if (daysFromLastPayment >= 180)
                return PastDue.HundredAndEighty;

            return null;
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
