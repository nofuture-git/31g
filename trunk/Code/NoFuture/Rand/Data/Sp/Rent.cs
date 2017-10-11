using System;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Domus.Pneuma;
using NoFuture.Shared;
using NoFuture.Util;
using NoFuture.Util.Math;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class Rent : ReceivableBase
    {
        #region fields
        private readonly Pecuniam _proRatedAmt;
        private readonly DateTime _dtOfFirstFullRentDue;
        private int _dayOfMonthRentDue = 1;
        #endregion

        #region properties
        public Pecuniam Deposit { get; private set; }
        public int LeaseTermInMonths { get; }
        public Pecuniam MonthlyPmt { get; }
        public DateTime LeaseExpiry { get; }
        public Identifier Id { get; }
        #endregion

        #region ctors

        public Rent(Identifier property, DateTime signing, int forMonths, Pecuniam monthlyRent, Pecuniam deposit): base(signing)
        {
            //calc number of days till the first day of the next month
            if (signing.Day != 1)
            {
                _dtOfFirstFullRentDue = signing.Month == 12
                    ? new DateTime(signing.Year + 1, 1, _dayOfMonthRentDue)
                    : new DateTime(signing.Year, signing.Month + 1, _dayOfMonthRentDue);
                
                var tsTillNextMonth = _dtOfFirstFullRentDue - signing;
                _proRatedAmt = new Pecuniam(Math.Round(monthlyRent.Amount/30 * (tsTillNextMonth.Days - 1), 2));
            }
            else
            {
                _proRatedAmt = Pecuniam.Zero;
                _dtOfFirstFullRentDue = signing;
            }
            LeaseExpiry = _dtOfFirstFullRentDue.AddMonths(forMonths);
            var fullTermAmt = _proRatedAmt + new Pecuniam(monthlyRent.Amount*forMonths);
            base.TradeLine.Balance.AddTransaction(signing, fullTermAmt, null, "Lease Signing");
            base.TradeLine.FormOfCredit = FormOfCredit.None;
            LeaseTermInMonths = forMonths;
            Deposit = deposit;
            MonthlyPmt = monthlyRent;
            Id = property;
            Description = $"{forMonths}-Month Lease";
            if (property is ResidentAddress)
            {
                ((ResidentAddress) property).IsLeased = true;
            }
        }
        #endregion

        #region methods

        public override Pecuniam GetValueAt(DateTime dt)
        {
            //when date is prior to signing 
            return dt.ComparedTo(TradeLine.OpennedDate) == ChronoCompare.Before
                ? Pecuniam.Zero
                : TradeLine.Balance.GetCurrent(dt, 0);
        }

        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var e = -1*GetExpectedTotalRent(dt).Amount;
            dt = dt.ComparedTo(TradeLine.OpennedDate) == ChronoCompare.SameTime 
                ? dt.AddDays(1) 
                : dt;
            var pd = TradeLine.Balance.GetDebitSum(
                new Tuple<DateTime, DateTime>(TradeLine.OpennedDate, dt));
            return new Pecuniam(e - pd.Amount);
        }

        public void PayRent(DateTime dt, Pecuniam amt, string note = "")
        {
            TradeLine.Balance.AddTransaction(dt, amt.Neg, Pecuniam.Zero, note);
        }

        protected internal Pecuniam GetExpectedTotalRent(DateTime dt)
        {
            //when date is prior to signing 
            if (dt.ComparedTo(TradeLine.OpennedDate) == ChronoCompare.Before)
                return Pecuniam.Zero;

            //when between signing and first months rent
            if(dt.ComparedTo(_dtOfFirstFullRentDue) == ChronoCompare.Before)
                return _proRatedAmt;

            var numOfRentPmts = CountOfWholeCalendarMonthsBetween(TradeLine.OpennedDate, dt, _dayOfMonthRentDue);

            //don't let calc exceed contract limit
            numOfRentPmts = numOfRentPmts > LeaseTermInMonths ? LeaseTermInMonths : numOfRentPmts;

            return new Pecuniam(MonthlyPmt.Amount * (numOfRentPmts+1)) + _proRatedAmt;
        }

        public static int CountOfWholeCalendarMonthsBetween(DateTime d1, DateTime d2, int dayOfMonthRentDue = 1)
        {
            if (d1.Date.ComparedTo(d2.Date) == ChronoCompare.SameTime)
                return 0;

            DateTime olderDt;
            DateTime newerDt;
            if (d1.ComparedTo(d2) == ChronoCompare.Before)
            {
                olderDt = d1;
                newerDt = d2;
            }
            else
            {
                olderDt = d2;
                newerDt = d1;
            }
            //back newer date to first day of its month
            newerDt = new DateTime(newerDt.Year, newerDt.Month, dayOfMonthRentDue);

            var countOfMonths = 0;
            while (newerDt.ComparedTo(olderDt) == ChronoCompare.After)
            {
                newerDt = newerDt.Month == 1
                    ? new DateTime(newerDt.Year - 1, 12, dayOfMonthRentDue)
                    : new DateTime(newerDt.Year, newerDt.Month - 1, dayOfMonthRentDue);
                if (newerDt.ComparedTo(olderDt) == ChronoCompare.After 
                    || newerDt.ComparedTo(olderDt) == ChronoCompare.SameTime)
                    countOfMonths += 1;
            }

            return countOfMonths;
        }

        /// <summary>
        /// Factory method to generate a <see cref="Rent"/> instance at random.
        /// </summary>
        /// <param name="property">The property identifier on which is being leased</param>
        /// <param name="homeDebtFactor">The home debt factor based on the renter's age, gender, edu, etc.</param>
        /// <param name="renterPersonality">Optional, used when creating a history of payments.</param>
        /// <param name="stdDevAsPercent">Optional, the stdDev around the mean.</param>
        /// <returns></returns>
        public static Rent GetRandomRentWithHistory(Identifier property, double homeDebtFactor, Personality renterPersonality = null,
            double stdDevAsPercent = NorthAmericanWealth.DF_STD_DEV_PERCENT)
        {
            //create a rent object
            renterPersonality = renterPersonality ?? new Personality();
            var avgRent = (double) Rent.GetAvgAmericanRentByYear(null).Amount;
            var randRent = new Pecuniam(
                (decimal)
                    NorthAmericanWealth.GetRandomFactorValue(NorthAmericanWealth.FactorTables.HomeDebt, homeDebtFactor,
                        stdDevAsPercent, avgRent));
            var randTerm = Etx.DiscreteRange(new[] {24, 18, 12, 6});
            var randDate = Etx.Date(0, DateTime.Today.AddDays(-2), true);
            var randDepositAmt = (int) Math.Round((randRent.Amount - randRent.Amount%250)/2);
            var randDeposit = new Pecuniam(randDepositAmt);

            var rent = new Rent(property, randDate, randTerm, randRent, randDeposit);

            //create payment history until current
            var firstPmt = rent.GetMinPayment(randDate);
            rent.PayRent(randDate.AddDays(1), firstPmt, "First Rent Payment");

            var rentDueDate = randDate.Month == 12
                ? new DateTime(randDate.Year + 1, 1, 1)
                : new DateTime(randDate.Year, randDate.Month + 1, 1);

            while (rentDueDate < DateTime.Today)
            {
                var paidRentOn = rentDueDate;
                //move the date rent was paid to some late-date when person acts irresponsible
                if (renterPersonality.GetRandomActsIrresponsible())
                    paidRentOn = paidRentOn.AddDays(Etx.IntNumber(5, 15));

                rent.PayRent(paidRentOn, randRent, Opes.GetPaymentNote(rent.Id));
                rentDueDate = rentDueDate.AddMonths(1);
            }
            return rent;
        }

        /// <summary>
        /// http://www.deptofnumbers.com/rent/us/
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Pecuniam GetAvgAmericanRentByYear(DateTime? dt)
        {
            var eq = new LinearEquation {Intercept = -13340, Slope = 7.1091};

            var year = dt.GetValueOrDefault(DateTime.Today).ToDouble();

            return new Pecuniam( (decimal)eq.SolveForY(year));
        }
        #endregion
    }
}
