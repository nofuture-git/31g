using System;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represents a Rent or Lease
    /// </summary>
    [Serializable]
    public class Rent : NamedReceivable
    {
        #region fields
        private readonly Pecuniam _proRatedAmt;
        private readonly DateTime _dtOfFirstFullRentDue;
        private int _dayOfMonthRentDue = 1;
        #endregion

        #region properties
        public Pecuniam Deposit { get; }
        public int LeaseTermInMonths { get; }
        public Pecuniam MonthlyPmt { get; }
        public DateTime? LeaseExpiry => Terminus;
        public Identifier Id { get; }
        public DateTime SigningDate => Inception;

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
            Terminus = _dtOfFirstFullRentDue.AddMonths(forMonths);
            var fullTermAmt = _proRatedAmt + new Pecuniam(monthlyRent.Amount*forMonths);
            Balance.AddTransaction(signing, fullTermAmt, new Mereo("Lease Signing"));
            LeaseTermInMonths = forMonths;
            Deposit = deposit;
            MonthlyPmt = monthlyRent;
            Id = property;
            DueFrequency = DefaultDueFrequency;
            FormOfCredit = Enums.FormOfCredit.None;

        }
        #endregion

        #region methods

        public override Pecuniam GetValueAt(DateTime dt)
        {
            //when date is prior to signing 
            return dt < Inception
                ? Pecuniam.Zero
                : Balance.GetCurrent(dt, 0);
        }

        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var e = -1*GetExpectedTotalRent(dt).Amount;
            dt = dt == Inception
                ? dt.AddDays(1) 
                : dt;
            var pd = Balance.GetDebitSum(
                new Tuple<DateTime, DateTime>(Inception, dt));
            return new Pecuniam(e - pd.Amount);
        }

        /// <summary>
        /// Helper method to put functionality in common vernacular 
        /// - is the exact same as <see cref="ITransactionable.AddNegativeValue"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="amount"></param>
        /// <param name="note"></param>
        public void PayRent(DateTime dt, Pecuniam amount, IVoca note = null)
        {
            AddNegativeValue(dt, amount, note);
        }

        protected internal Pecuniam GetExpectedTotalRent(DateTime dt)
        {
            //when date is prior to signing 
            if (dt < Inception)
                return Pecuniam.Zero;

            //when between signing and first months rent
            if(dt < _dtOfFirstFullRentDue)
                return _proRatedAmt;

            var numOfRentPmts = CountOfWholeCalendarMonthsBetween(Inception, dt, _dayOfMonthRentDue);

            //don't let calc exceed contract limit
            numOfRentPmts = numOfRentPmts > LeaseTermInMonths ? LeaseTermInMonths : numOfRentPmts;

            return new Pecuniam(MonthlyPmt.Amount * (numOfRentPmts+1)) + _proRatedAmt;
        }

        public static int CountOfWholeCalendarMonthsBetween(DateTime d1, DateTime d2, int dayOfMonthRentDue = 1)
        {
            if (d1.Date == d2.Date)
                return 0;

            DateTime olderDt;
            DateTime newerDt;
            if (d1 < d2)
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
            while (newerDt > olderDt)
            {
                newerDt = newerDt.Month == 1
                    ? new DateTime(newerDt.Year - 1, 12, dayOfMonthRentDue)
                    : new DateTime(newerDt.Year, newerDt.Month - 1, dayOfMonthRentDue);
                if (newerDt > olderDt || newerDt == olderDt)
                    countOfMonths += 1;
            }

            return countOfMonths;
        }

        public override string ToString()
        {
            return new Tuple<Pecuniam, DateTime, DateTime?>(MonthlyPmt, Inception, Terminus).ToString();
        }

        /// <summary>
        /// Factory method to generate a <see cref="Rent"/> instance at random.
        /// </summary>
        /// <param name="property">The property identifier on which is being leased</param>
        /// <param name="totalYearlyRent">
        /// Optional, allows the calling assembly to specify this, default 
        /// is calculated from <see cref="GetAvgAmericanRentByYear"/>
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static Rent RandomRent(Identifier property = null, double? totalYearlyRent = null)
        {
            var avgRent = totalYearlyRent ?? 0D;
            var randRent = Pecuniam.Zero;
            if (avgRent == 0D)
            {
                avgRent = (double) GetAvgAmericanRentByYear(null).Amount;
                var stdOfAvgRent = avgRent * 0.43759;
                var lower = (int) Math.Round(avgRent - (stdOfAvgRent * 3));
                var upper = (int) Math.Round(avgRent + (stdOfAvgRent * 3));
                randRent = Pecuniam.RandomPecuniam(lower, upper, 100);
            }
            else
            {
                randRent = avgRent.ToPecuniam();
            }
            var randTerm = Etx.RandomPickOne(new[] { 24, 18, 12, 6 });
            var randDate = Etx.RandomDate(0, DateTime.Today.AddDays(-2), true);
            var randDepositAmt = (int)Math.Round((randRent.Amount - randRent.Amount % 250) / 2);
            var randDeposit = new Pecuniam(randDepositAmt);

            var rent = new Rent(property, randDate, randTerm, randRent, randDeposit);
            return rent;
        }

        /// <summary>
        /// Factory method to generate a <see cref="Rent"/> instance at random with a payment history.
        /// </summary>
        /// <param name="property">The property identifier on which is being leased</param>
        /// <param name="totalYearlyRent">
        /// Optional, allows the calling assembly to specify this, default 
        /// is calculated from <see cref="GetAvgAmericanRentByYear"/>
        /// </param>
        /// <param name="randomActsIrresponsible">Optional, used when creating a more colorful history of payments.</param>
        /// <returns></returns>
        [RandomFactory]
        public static Rent RandomRentWithHistory(Identifier property = null, double? totalYearlyRent = null,
            Func<bool> randomActsIrresponsible = null)
        {
            //create a rent object
            randomActsIrresponsible = randomActsIrresponsible ?? (() => false);

            var rent = RandomRent(property, totalYearlyRent);
            var randDate = rent.SigningDate;
            var randRent = rent.MonthlyPmt;
            //create payment history until current
            var firstPmt = rent.GetMinPayment(randDate);
            var note = property == null ? null : new Mereo(property.ToString());
            rent.PayRent(randDate.AddDays(1), firstPmt, note);

            var rentDueDate = randDate.Month == 12
                ? new DateTime(randDate.Year + 1, 1, 1)
                : new DateTime(randDate.Year, randDate.Month + 1, 1);

            while (rentDueDate < DateTime.Today)
            {
                var paidRentOn = rentDueDate;
                //move the date rent was paid to some late-date when person acts irresponsible
                if (randomActsIrresponsible())
                    paidRentOn = paidRentOn.AddDays(Etx.RandomInteger(5, 15));

                note = rent.Id != null ? new Mereo(rent.Id.ToString()) : null;

                rent.PayRent(paidRentOn, randRent, note);
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
            var eq = new LinearEquation(7.1091, -13340);

            var year = dt.GetValueOrDefault(DateTime.Today).ToDouble();

            return new Pecuniam((decimal)eq.SolveForY(year));
        }
        #endregion
    }
}
