using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Cc;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represents a loan which is borrowed on some kind of secured property
    /// </summary>
    [Serializable]
    public class SecuredFixedRateLoan : FixedRateLoan
    {
        /// <summary>
        /// Src https://data.worldbank.org/indicator/NY.GDP.MKTP.KD 1960-2016
        /// </summary>
        public const float AVG_GDP_GROWTH_RATE = 0.031046655f;

        private Pecuniam _monthlyPayment;
        private float _rate;
        private readonly int _termInYears;

        #region ctors
        public SecuredFixedRateLoan(Identifier property, DateTime openedDate, float minPaymentRate, Pecuniam amount = null)
            : base(openedDate, minPaymentRate, amount)
        {
            PropertyId = property;
        }

        public SecuredFixedRateLoan(Identifier property, DateTime openedDate, Pecuniam totalCost, float rate, int termInYears) : base(
            openedDate, CreditCardAccount.DF_MIN_PMT_RATE, totalCost)
        {
            PropertyId = property;
            if (totalCost == null || totalCost == Pecuniam.Zero)
                throw new ArgumentException("You must pass in a valid amount.");

            //interest rate must be a positive number
            _rate = Math.Abs(rate);

            //handle if caller passes in rate like 5.5 meaning they wanted 0.055
            if (_rate > 1)
                _rate = Convert.ToSingle(Math.Round(_rate / 100, 4));

            //calc the monthly payment
            var fv = totalCost.Amount.PerDiemInterest(rate, DaysPerYear * termInYears, DaysPerYear);
            _monthlyPayment = new Pecuniam(Math.Round(fv / (termInYears * 12), 2));
            _minPaymentRate = fv == 0
                ? CreditCardAccount.DF_MIN_PMT_RATE
                : (float)Math.Round(_monthlyPayment.Amount / fv, 6);
            _termInYears = termInYears;
        }
        #endregion

        #region properties
        /// <summary>
        /// The monthly installment payments
        /// </summary>
        public virtual Pecuniam MonthlyPayment
        {
            get => _monthlyPayment;
            set => _monthlyPayment = value;
        }

        /// <summary>
        /// The property (real or personal) on which the loan is secured
        /// </summary>
        public Identifier PropertyId { get; set; }

        /// <summary>
        /// The term of the loan in whole years
        /// </summary>
        public virtual int TermInYears => _termInYears;

        public override float Rate
        {
            get => _rate;
            set
            {
                var temp = value;
                var reCalc = Math.Abs(_rate - temp) > 0.00001 && _termInYears > 0;
                _rate = temp;
                if (reCalc)
                    CalcFromRate();
            }
        }

        /// <summary>
        /// The average estimated rate at which the 
        /// value of the underlying asset is 
        /// expected to grow\shrink if it where sold on the open market
        /// (default is <see cref="AVG_GDP_GROWTH_RATE"/>).
        /// </summary>
        public float ExpectedMarketValueRate { get; set; } = AVG_GDP_GROWTH_RATE;

        #endregion

        #region methods

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x?.Replace(",", "").Replace(" ", ""), txtCase);
            var propertyName = Name;
            propertyName += PropertyId?.ToString();
            var itemData = new Dictionary<string, object>
            {
                {textFormat(propertyName + "RemainingBalance"), Value},
                {textFormat(propertyName + "InterestRate"), Rate},
                {textFormat(propertyName + "MonthlyPayment"), MonthlyPayment.ToString()},
                {textFormat(propertyName + nameof(TermInYears)), TermInYears},
                {textFormat(propertyName + "PurchaseDate"), Inception.ToString("s")}
            };

            return itemData;
        }

        /// <summary>
        /// The future value of the loan.
        /// </summary>
        /// <returns></returns>
        protected internal virtual decimal GetFutureValue(double numOfDays)
        {
            var fv = GetValueAt(Inception).Amount.PerDiemInterest(_rate, numOfDays, DaysPerYear);
            return fv;
        }

        /// <summary>
        /// Calculates an estimated market value as the purchase price grown at 
        /// the <see cref="rate"/> at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt">
        /// The date of query
        /// </param>
        /// <param name="rate">
        /// Optional, will use <see cref="ExpectedMarketValueRate"/> if null.
        /// The rate at which the value increases\decreases over time.
        /// </param>
        /// <returns></returns>
        public virtual Pecuniam GetEstimatedMarketValueAt(DateTime? dt, float? rate = null)
        {
            var qDt = dt ?? DateTime.Today;
            var pDt = Inception;

            var atRate = rate ?? ExpectedMarketValueRate;

            if (qDt < pDt)
                return OriginalBorrowAmount;

            var numDays = (qDt - pDt).TotalDays;

            var marketValue =
                OriginalBorrowAmount.GetAbs().Amount.PerDiemInterest(atRate, numDays, DaysPerYear);

            return marketValue.ToPecuniam();
        }

        /// <summary>
        /// Helper method to (re)assign instance fields whenever a change in rate is noticed.
        /// </summary>
        protected internal void CalcFromRate()
        {
            var fv = GetFutureValue(DaysPerYear * _termInYears);
            _monthlyPayment = new Pecuniam(Math.Round(fv / (_termInYears * 12), 2));
            _minPaymentRate = fv == 0
                ? CreditCardAccount.DF_MIN_PMT_RATE
                : (float)Math.Round(_monthlyPayment.Amount / fv, 6);
        }

        /// <summary>
        /// Produces a random <see cref="SecuredFixedRateLoan"/>.
        /// </summary>
        /// <param name="remainingCost">The balance which currently remains.</param>
        /// <param name="totalCost">
        ///     The original value of the loan, the difference between 
        ///     this and the <see cref="remainingCost"/> determines how far in the past the loan would
        ///     have been openned.
        /// </param>
        /// <param name="rate">The interest rate</param>
        /// <param name="termInYears"></param>
        /// <param name="property">The property on which the loan is secured.</param>
        /// <returns></returns>
        [RandomFactory]
        public static SecuredFixedRateLoan RandomSecuredFixedRateLoan(Pecuniam remainingCost = null,
            Pecuniam totalCost = null,
            float rate = Mortgage.AVG_GDP_GROWTH_RATE, int termInYears = 5, Identifier property = null)
        {
            //HACK - checking type-name as string to avoid adding a ref to Rand.Geo
            var isMortgage = String.Equals(property?.GetType().Name ?? "", "PostalAddress",
                StringComparison.OrdinalIgnoreCase);

            //if no or nonsense values given, change to some default
            if (totalCost == null || totalCost < Pecuniam.Zero)
                totalCost = Pecuniam.RandomPecuniam(1000, 100000, 100);
            if (remainingCost == null || remainingCost < Pecuniam.Zero)
                remainingCost = Pecuniam.Zero;

            //remaining must always be less than the total 
            if (remainingCost > totalCost)
                totalCost = remainingCost + Pecuniam.RandomPecuniam(1000, 10000);

            termInYears = Math.Abs(termInYears);
            if (termInYears == 0)
                termInYears = 5;

            //given this value and rate - calc the timespan needed to have aquired this amount of equity
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);
            var loan = new SecuredFixedRateLoan(property, firstOfYear, totalCost, rate, termInYears)
            {
                DueFrequency = new TimeSpan(30, 0, 0, 0)
            };

            var dtIncrement = firstOfYear.AddMonths(1);
            while (loan.GetValueAt(dtIncrement) > remainingCost)
            {
                if (dtIncrement > DateTime.UtcNow.AddYears(termInYears))
                    break;
                loan.MakeAPayment(dtIncrement, loan.MonthlyPayment);
                dtIncrement = dtIncrement.AddMonths(1);
            }

            //repeat process from calc'ed past date to create a history
            var calcPurchaseDt = DateTime.Today.AddDays(-1 * (dtIncrement - firstOfYear).Days);
            loan = isMortgage
                    ? new Mortgage(property, calcPurchaseDt, rate, totalCost, termInYears)
                    : new SecuredFixedRateLoan(property, calcPurchaseDt, totalCost, rate, termInYears);

            return loan;
        }

        /// <summary>
        /// Same as its counterpart <see cref="RandomSecuredFixedRateLoan"/> only it also produces a history of transactions.
        /// </summary>
        /// <param name="remainingCost">The balance which currently remains.</param>
        /// <param name="totalCost">
        ///     The original value of the loan, the difference between 
        ///     this and the <see cref="remainingCost"/> determines how much history is needed.
        /// </param>
        /// <param name="rate">The interest rate</param>
        /// <param name="termInYears"></param>
        /// <param name="property">The property on which the loan is secured.</param>
        /// <param name="randomActsIrresponsible">Optional, used when creating a more colorful history of payments.</param>
        /// <returns></returns>
        [RandomFactory]
        public static SecuredFixedRateLoan RandomSecuredFixedRateLoanWithHistory(Pecuniam remainingCost = null,
            Pecuniam totalCost = null, float rate = AVG_GDP_GROWTH_RATE, int termInYears = 5,
            Identifier property = null,
            Func<bool> randomActsIrresponsible = null)
        {
            var loan = RandomSecuredFixedRateLoan(remainingCost, totalCost, rate, termInYears, property);
            var minPmt = loan.MonthlyPayment;

            //makes the fake history more colorful
            var score = Etx.RandomDouble();
            randomActsIrresponsible =
                randomActsIrresponsible ?? (() => Etx.RandomValueInNormalDist(score, 0.33334D) > 0);

            var dtIncrement = loan.Inception.AddMonths(1);
            while (loan.GetValueAt(dtIncrement) > remainingCost)
            {
                if (dtIncrement >= DateTime.UtcNow)
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

                loan.MakeAPayment(paidOnDate, minPmt);
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