using System;
using System.Collections.Generic;
using NoFuture.Rand.Com;
using NoFuture.Shared;
using NoFuture.Util.Math;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Domus.Pneuma;

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
            TradeLine.Balance.AddTransaction(dt, amt.Neg, fee, note);
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
            if (val == Pecuniam.Zero)
                return false;
            TradeLine.Balance.AddTransaction(dt, val.Abs, fee, note);
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
        /// Produces a random <see cref="SecuredFixedRateLoan"/> with history.
        /// </summary>
        /// <param name="property">The property on which the loan is secured.</param>
        /// <param name="remainingCost">The balance which currently remains.</param>
        /// <param name="totalCost">
        /// The original value of the loan, the difference between 
        /// this and the <see cref="remainingCost"/> determines how much history is needed.
        /// </param>
        /// <param name="rate">The interest rate</param>
        /// <param name="termInYears"></param>
        /// <param name="minPmt"></param>
        /// <param name="borrowerPersonality">Optional, used when creating a history of payments.</param>
        /// <returns></returns>
        public static SecuredFixedRateLoan GetRandomLoanWithHistory(Identifier property, Pecuniam remainingCost,
            Pecuniam totalCost, float rate, int termInYears, out Pecuniam minPmt, IPersonality borrowerPersonality = null)
        {
            //if no or nonsense values given, change to some default
            if (totalCost == null || totalCost < Pecuniam.Zero)
                totalCost = new Pecuniam(2000);
            if (remainingCost == null || remainingCost < Pecuniam.Zero)
                remainingCost = Pecuniam.Zero;

            //remaining must always be less than the total 
            if (remainingCost > totalCost)
                totalCost = remainingCost + Pecuniam.GetRandPecuniam(1000, 3000);

            //interest rate must be a positive number
            rate = Math.Abs(rate);

            //handle if caller passes in rate like 5.5 meaning they wanted 0.055
            if (rate > 1)
                rate = Convert.ToSingle(Math.Round(rate/100, 4));

            //makes the fake history more colorful
            borrowerPersonality = borrowerPersonality ?? new Personality();

            //calc the monthly payment
            var fv = totalCost.Amount.PerDiemInterest(rate, Constants.TropicalYear.TotalDays * termInYears);
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
                if (borrowerPersonality.GetRandomActsIrresponsible())
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
            var propertyAddress = property as ResidentAddress;
            loan.Lender = propertyAddress == null
                ? Bank.GetRandomBank(null)
                : Bank.GetRandomBank(propertyAddress.HomeCityArea);
            return loan;
        }

        #endregion
    }
}
