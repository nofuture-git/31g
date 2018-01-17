using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
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
        /// Produces a random <see cref="SecuredFixedRateLoan"/>.
        /// </summary>
        /// <param name="property">The property on which the loan is secured.</param>
        /// <param name="remainingCost">The balance which currently remains.</param>
        /// <param name="totalCost">
        /// The original value of the loan, the difference between 
        /// this and the <see cref="remainingCost"/> determines how far in the past the loan would
        /// have been openned.
        /// </param>
        /// <param name="rate">The interest rate</param>
        /// <param name="termInYears"></param>
        /// <param name="minPmt"></param>
        /// <returns></returns>
        public static SecuredFixedRateLoan GetRandomLoan(Identifier property, Pecuniam remainingCost,
            Pecuniam totalCost, float rate, int termInYears, out Pecuniam minPmt)
        {
            //HACK - checking type-name as string to avoid adding a ref to Rand.Geo
            var isMortgage = String.Equals(property?.GetType()?.Name ?? "", "PostalAddress",
                StringComparison.OrdinalIgnoreCase);

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
                rate = Convert.ToSingle(Math.Round(rate / 100, 4));

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
            loan = isMortgage
                ? new Mortgage(property, calcPurchaseDt, rate, totalCost)
                : new SecuredFixedRateLoan(property, calcPurchaseDt, minPmtRate, totalCost)
                {
                    Rate = rate
                };

            loan.FormOfCredit = isMortgage
                ? FormOfCredit.Mortgage
                : FormOfCredit.Installment;

            return loan;
        }

        /// <summary>
        /// Same as its counterpart <see cref="GetRandomLoan"/> only it also produces a history of transactions.
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
        /// <param name="randomActsIrresponsible">Optional, used when creating a more colorful history of payments.</param>
        /// <returns></returns>
        public static SecuredFixedRateLoan GetRandomLoanWithHistory(Identifier property, Pecuniam remainingCost,
            Pecuniam totalCost, float rate, int termInYears, out Pecuniam minPmt, Func<bool> randomActsIrresponsible = null)
        {

            var loan = GetRandomLoan(property, remainingCost, totalCost, rate, termInYears, out minPmt);

            var pmtNote = new Mereo(property?.ToString() ?? nameof(property));

            //makes the fake history more colorful
            randomActsIrresponsible = randomActsIrresponsible ?? (() => false);

            var dtIncrement = loan.Inception.AddMonths(1);
            while (loan.GetValueAt(dtIncrement) > remainingCost)
            {
                if (dtIncrement >= DateTime.Now)
                    break;
                var paidOnDate = dtIncrement;
                if (randomActsIrresponsible())
                    paidOnDate = paidOnDate.AddDays(Etx.RandomInteger(5, 15));

                //is this the payoff
                var isPayoff = loan.GetValueAt(paidOnDate) <= minPmt;
                if (isPayoff)
                {
                    minPmt = loan.GetValueAt(paidOnDate);
                }

                loan.Push(paidOnDate, minPmt, pmtNote, Pecuniam.Zero);
                if (isPayoff)
                {
                    loan.Terminus = paidOnDate;
                    loan.Closure = ClosedCondition.ClosedWithZeroBalance;
                    break;
                }
                dtIncrement = dtIncrement.AddMonths(1);
            }
            return loan;
        }

        #endregion
    }
}