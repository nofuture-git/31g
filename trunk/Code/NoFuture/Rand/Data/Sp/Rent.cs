using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents a Rent or Lease
    /// </summary>
    [Serializable]
    public class Rent : Pondus
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
        public DateTime LeaseExpiry { get; }
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
            LeaseExpiry = _dtOfFirstFullRentDue.AddMonths(forMonths);
            var fullTermAmt = _proRatedAmt + new Pecuniam(monthlyRent.Amount*forMonths);
            Balance.AddTransaction(signing, fullTermAmt, new Mereo("Lease Signing"));
            FormOfCredit = FormOfCredit.None;
            LeaseTermInMonths = forMonths;
            Deposit = deposit;
            MonthlyPmt = monthlyRent;
            Id = property;

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

        public void PayRent(DateTime dt, Pecuniam amt, IMereo note = null)
        {
            Balance.AddTransaction(dt, amt.Neg, note, Pecuniam.Zero);
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
