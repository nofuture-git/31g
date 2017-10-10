using System;
using System.Collections.Generic;
using NoFuture.Rand.Com;
using NoFuture.Shared;
using NoFuture.Util.Math;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp //Sequere pecuniam
{
    [Serializable]
    public abstract class LoanBase<T> : ReceivableBase, ILoan, ITransactionable 
    {
        #region ctors
        protected LoanBase(DateTime openedDate, float minPaymentRate):base(openedDate)
        {
            MinPaymentRate = minPaymentRate;
        }
        #endregion

        #region properties
        public float MinPaymentRate { get; set; }
        public T Rate { get; set; }
        public IFirm Lender { get; set; }
        #endregion

        #region methods
        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var bal = GetValueAt(dt);
            if (bal < Pecuniam.Zero)
                return Pecuniam.Zero;

            var amt = bal.Amount * Convert.ToDecimal(MinPaymentRate);
            return new Pecuniam(Math.Round(amt, 2)).Neg;
        }

        /// <summary>
        /// Applied a negative valued transaction against the balance.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="amt"></param>
        /// <param name="fee"></param>
        /// <param name="note"></param>
        public void Push(DateTime dt, Pecuniam amt, Pecuniam fee = null, string note = null)
        {
            if (amt == Pecuniam.Zero)
                return;
            TradeLine.Balance.AddTransaction(dt, amt.Neg, Pecuniam.Zero, note);
        }

        /// <summary>
        /// Applies a positive valued transaction against the balance
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="val"></param>
        /// <param name="fee"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public virtual bool Pop(DateTime dt, Pecuniam val, Pecuniam fee = null, string note = null)
        {
            TradeLine.Balance.AddTransaction(dt, val.Abs, Pecuniam.Zero, note);
            return true;
        }
        #endregion
    }

    [Serializable]
    public class FixedRateLoan : LoanBase<float>
    {
        #region ctors

        public FixedRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(openedDate, minPaymentRate)
        {
            if (amt != null && amt.Amount != 0)
                _tl.Balance.AddTransaction(openedDate, amt.Abs, Pecuniam.Zero, "Initial Transaction");
        }

        #endregion

        #region methods
        public override Pecuniam GetValueAt(DateTime dt)
        {
            return TradeLine.Balance.GetCurrent(dt, Rate);
        }
        #endregion
    }

    [Serializable]
    public class VariableRateLoan : LoanBase<Dictionary<DateTime, float>>
    {
        #region ctors

        public VariableRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(openedDate, minPaymentRate)
        {
            if (amt != null && amt.Amount != 0)
                _tl.Balance.AddTransaction(openedDate, amt.Abs);
        }

        #endregion

        #region methods
        public override Pecuniam GetValueAt(DateTime dt)
        {
            return TradeLine.Balance.GetCurrent(dt, Rate);
        }
        #endregion
    }

    [Serializable]
    public class SecuredFixedRateLoan : FixedRateLoan
    {
        public SecuredFixedRateLoan(Identifier property, DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(openedDate, minPaymentRate, amt)
        {
            PropertyId = property;
        }

        public Identifier PropertyId { get; set; }

        #region methods

        /// <summary>
        /// Produces a <see cref="SecuredFixedRateLoan"/> with history.
        /// </summary>
        /// <param name="borrower"></param>
        /// <param name="property"></param>
        /// <param name="remainingCost"></param>
        /// <param name="totalCost"></param>
        /// <param name="rate"></param>
        /// <param name="termInYears"></param>
        /// <param name="minPmt"></param>
        /// <returns></returns>
        public static SecuredFixedRateLoan GetRandomLoanWithHistory(IPerson borrower, Identifier property, Pecuniam remainingCost,
            Pecuniam totalCost, float rate, int termInYears, out Pecuniam minPmt)
        {
            //if no or nonsense values given, change to some default
            if (totalCost == null || totalCost < Pecuniam.Zero)
                totalCost = new Pecuniam(2000);
            if (remainingCost == null || remainingCost < Pecuniam.Zero)
                remainingCost = Pecuniam.Zero;

            rate = Math.Abs(rate);

            //calc the monthly payment
            var fv = remainingCost.Amount.PerDiemInterest(rate, Constants.TropicalYear.TotalDays * termInYears);
            minPmt = new Pecuniam(Math.Round(fv / (termInYears * 12), 2));
            var minPmtRate = fv == 0 ? CreditCardAccount.DF_MIN_PMT_RATE : (float)Math.Round(minPmt.Amount / fv, 6);

            //given this value and rate - calc the timespan needed to have aquired this amount of equity
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var loan = new SecuredFixedRateLoan(property, firstOfYear, minPmtRate, totalCost)
            {
                Rate = rate
            };

            var dtIncrement = firstOfYear.AddMonths(1);
            while (loan.GetValueAt(dtIncrement) > remainingCost)
            {
                if (dtIncrement > DateTime.Now.AddYears(termInYears))
                    break;
                loan.Push(dtIncrement, minPmt);
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //repeat process from calc'ed past date to create a history
            var calcPurchaseDt = DateTime.Today.AddDays(-1 * (dtIncrement - firstOfYear).Days);
            loan = new SecuredFixedRateLoan(property, calcPurchaseDt, minPmtRate, totalCost)
            {
                Rate = rate
            };

            var pmtNote = Opes.GetPaymentNote(property);

            dtIncrement = calcPurchaseDt.AddMonths(1);
            while (loan.GetValueAt(dtIncrement) > remainingCost)
            {
                if (dtIncrement >= DateTime.Now)
                    break;
                var paidOnDate = dtIncrement;
                if (borrower != null && borrower.Personality.GetRandomActsIrresponsible())
                    paidOnDate = paidOnDate.AddDays(Etx.IntNumber(5, 15));

                //is this the payoff
                var isPayoff = loan.GetValueAt(dtIncrement) <= minPmt;
                if (isPayoff)
                    minPmt = loan.GetValueAt(dtIncrement);

                loan.Push(paidOnDate, minPmt, Pecuniam.Zero, pmtNote);
                if (isPayoff)
                    break;
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //assign boilerplate props
            loan.Lender = borrower == null
                ? Bank.GetRandomBank(null)
                : Bank.GetRandomBank(borrower?.Address?.HomeCityArea);
            return loan;
        }

        #endregion
    }
}
