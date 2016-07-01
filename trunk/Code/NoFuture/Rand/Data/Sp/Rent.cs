using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Shared;
using NoFuture.Util;

namespace NoFuture.Rand.Data.Sp
{
    public class Rent : ReceivableBase
    {
        private Pecuniam _proRatedAmt;
        private DateTime _dtOfFirstFullRentDue;
        private DateTime _leaseExpiry;
        private Pecuniam _fullTermAmt;
        private int _dayOfMonthRentDue = 1;
        #region properties
        public Pecuniam Deposit { get; private set; }
        public int LeaseTermInMonths { get; private set; }
        public Pecuniam MonthlyPmt { get; private set; }
        #endregion

        #region ctors

        public Rent(DateTime signing, int forMonths, Pecuniam monthlyRent, Pecuniam deposit): base(signing)
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
                _proRatedAmt = new Pecuniam(0);
                _dtOfFirstFullRentDue = signing;
            }
            _leaseExpiry = _dtOfFirstFullRentDue.AddMonths(forMonths);
            _fullTermAmt = _proRatedAmt + new Pecuniam(monthlyRent.Amount*forMonths);
            base.TradeLine.Balance.Transactions.Add(new Transaction(signing, _fullTermAmt));

            LeaseTermInMonths = forMonths;
            Deposit = deposit;
            MonthlyPmt = monthlyRent;
        }
        #endregion

        #region methods

        public override Pecuniam GetCurrentBalance(DateTime dt)
        {
            //when date is prior to signing 
            if (dt.ComparedTo(TradeLine.OpennedDate) == ChronoCompare.Before)
                return new Pecuniam(0);

            return TradeLine.Balance.GetCurrent(dt, 0);
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

        public void PayRent(DateTime dt, Pecuniam amt)
        {
            if (amt.Amount > 0)
                amt = new Pecuniam(-1*amt.Amount);
            TradeLine.Balance.Transactions.Add(new Transaction(dt, amt));
        }

        protected internal Pecuniam GetExpectedTotalRent(DateTime dt)
        {
            //when date is prior to signing 
            if (dt.ComparedTo(TradeLine.OpennedDate) == ChronoCompare.Before)
                return new Pecuniam(0);

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
        #endregion
    }
}
