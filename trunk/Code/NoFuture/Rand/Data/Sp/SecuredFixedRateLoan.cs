using System;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Shared.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Data.Sp
{
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
                var isPayoff = loan.GetValueAt(paidOnDate) <= minPmt;
                if (isPayoff)
                {
                    minPmt = loan.GetValueAt(paidOnDate);
                }

                loan.Push(paidOnDate, minPmt, Pecuniam.Zero, pmtNote);
                if (isPayoff)
                {
                    loan.TradeLine.Closure = new TradelineClosure
                    {
                        ClosedDate = paidOnDate,
                        Condition = ClosedCondition.ClosedWithZeroBalance
                    };
                    break;
                }
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