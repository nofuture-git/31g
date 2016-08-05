using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Exceptions;
using NoFuture.Util.Math;

namespace NoFuture.Rand
{
    public interface IExchangeRate
    {
        CurrencyAbbrev CurrencyOut { get; }
        Pecuniam Exchange(Pecuniam p);
    }

    /// <summary>
    /// Basic Money object pattern 
    /// </summary>
    /// <remarks>
    /// Is Latin for Money
    /// </remarks>
    [Serializable]
    public class Pecuniam
    {
        #region fields
        private readonly Decimal _amount;
        private readonly CurrencyAbbrev _currencyAbbrev;
        #endregion

        #region ctors
        public Pecuniam(Decimal amount, CurrencyAbbrev c = CurrencyAbbrev.USD)
        {
            _amount = amount;
            _currencyAbbrev = c;
        }
        #endregion

        #region properties
        public Decimal Amount => _amount;
        public CurrencyAbbrev CurrencyAbbrev => _currencyAbbrev;
        public Pecuniam Abs => new Pecuniam(Math.Abs(_amount));
        public Pecuniam Neg => new Pecuniam(-1*Math.Abs(_amount));
        public static Pecuniam Zero => new Pecuniam(0.0M);
        #endregion

        #region overrides

        public override string ToString()
        {
            return $"{Amount} {CurrencyAbbrev}";
        }

        public override bool Equals(object obj)
        {
            var p1 = obj as Pecuniam;
            return p1?.Amount == Amount;
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }

        public static Pecuniam operator +(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount + pp2.Amount);
        }

        public static Pecuniam operator -(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount - pp2.Amount);
        }

        public static Pecuniam operator *(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount * pp2.Amount);
        }

        public static Pecuniam operator /(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount / pp2.Amount);
        }

        public static bool operator ==(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;

            return pp1.Amount == pp2.Amount;
        }

        public static bool operator !=(Pecuniam p1, Pecuniam p2)
        {
            return !(p1 == p2);
        }

        public static bool operator >(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount > pp2.Amount;
        }

        public static bool operator <(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount < pp2.Amount;
        }

        public static bool operator >=(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount >= pp2.Amount;
        }

        public static bool operator <=(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? Zero;
            var pp2 = p2 ?? Zero;
            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount <= pp2.Amount;
        }

        public static Pecuniam GetRandPecuniam(int min = 3, int max = 999, int wholeNumbersOf = 0)
        {
            var num = (double)Etx.IntNumber(min, max);

            if (wholeNumbersOf > 10)
                num = num - (num%wholeNumbersOf);
            else
            {
                num = Etx.RationalNumber(min, max);
            }

            return new Pecuniam((decimal) Math.Round(num,2));
        }
        #endregion
    }

    /// <summary>
    /// ISO 4217 Currency Codes
    /// </summary>
    [Serializable]
    public enum CurrencyAbbrev
    {
        USD,
        EUR,
        GBP,
        JPY,
        AUD,
        CAD,
        BRL,
        MXN,
        CNY,
    }

    public interface ITransaction
    {
        Guid UniqueId { get; }
        DateTime AtTime { get; }
        Pecuniam Cash { get; }
    }

    /// <summary>
    /// Single immutable money transaction
    /// </summary>
    [Serializable]
    public class Transaction : ITransaction
    {
        #region fields
        private readonly DateTime _atTime;
        private readonly Pecuniam _cash;
        private readonly Guid _guid = Guid.NewGuid();
        #endregion

        public Transaction(DateTime atTime, Pecuniam amt)
        {
            _atTime = atTime;
            _cash = amt;
        }

        #region properties
        public Guid UniqueId => _guid;
        public DateTime AtTime => _atTime;
        public Pecuniam Cash => _cash;

        #endregion

        #region overrides
        public override bool Equals(object obj)
        {
            if (Equals(obj, null))
                return false;
            var t = obj as Transaction;
            if (t == null)
                return false;
            return _guid.Equals(t.UniqueId);
        }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join(" ", $"[{UniqueId}]", $"{AtTime:yyyy-MM-dd}", $"{Cash.Amount}");
        }

        #endregion
    }

    /// <summary>
    /// Sorts by <see cref="ITransaction.AtTime"/>
    /// </summary>
    [Serializable]
    public class TransactionComparer : IComparer<ITransaction>
    {
        public int Compare(ITransaction x, ITransaction y)
        {
            return DateTime.Compare(x.AtTime, y.AtTime);
        }
    }

    /// <summary>
    /// Represents the basic money account.
    /// </summary>
    public interface IBalance
    {
        /// <summary>
        /// Sorted oldest (index 0) to most current (index Count - 1)
        /// </summary>
        List<ITransaction> Transactions { get; }

        /// <summary>
        /// Gets transactions which occured on or after <see cref="from"/> up to the <see cref="to"/>
        /// </summary>
        /// <param name="from">
        /// Transactions which occured exactly on this date WILL be included in the results.
        /// </param>
        /// <param name="to">
        /// Transactions which occured exactly 
        /// on this date will not be included unless <see cref="includeThoseOnToDate"/>
        /// is set to true.
        /// </param>
        /// <param name="includeThoseOnToDate"></param>
        /// <returns></returns>
        List<ITransaction> GetTransactionsBetween(DateTime from, DateTime to, bool includeThoseOnToDate = false);

        /// <summary>
        /// Returns a negative value being the sum of all payments-out between dates in <see cref="between"/>
        /// </summary>
        /// <param name="between"></param>
        /// <returns></returns>
        Pecuniam GetDebitSum(Tuple<DateTime, DateTime> between);

        /// <summary>
        /// Returns a positive value being the sum of all payments-in between dates in <see cref="between"/>
        /// </summary>
        /// <param name="between"></param>
        /// <returns></returns>
        Pecuniam GetCreditSum(Tuple<DateTime, DateTime> between);

        /// <summary>
        /// Gets the current balance up to the <see cref="dt"/> for the
        /// given rate of <see cref="rate"/>.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        Pecuniam GetCurrent(DateTime dt, float rate);

        /// <summary>
        /// Gets the current balance upt to the <see cref="dt"/> for the
        /// given rates in <see cref="variableRate"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="variableRate">
        /// The dictonary keys are the dates the rate values end on, NOT begin on.
        /// </param>
        /// <returns></returns>
        Pecuniam GetCurrent(DateTime dt, Dictionary<DateTime, float> variableRate);
    }

    [Serializable]
    public class Balance : IBalance
    {
        #region fields
        private readonly List<ITransaction>  _transactions = new List<ITransaction>();
        private readonly IComparer<ITransaction> _comparer = new TransactionComparer();
        private readonly Func<Decimal, bool> _debitOp = x => x < 0;
        private readonly Func<Decimal, bool> _creditOp = x => x > 0;
        #endregion

        #region properties
        public List<ITransaction> Transactions
        {
            get
            {
                _transactions.Sort(_comparer);
                return _transactions;
            }
        }
        #endregion

        #region methods

        public void AddTransaction(DateTime dt, Pecuniam amnt)
        {
            if (amnt == Pecuniam.Zero)
                return;
            while (_transactions.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
            {
                dt = dt.AddMilliseconds(10);
            }
            _transactions.Add(new Transaction(dt, amnt));
        }

        public Pecuniam GetDebitSum(Tuple<DateTime, DateTime> between)
        {
            return GetRangeSum(between, _debitOp);
        }

        public Pecuniam GetCreditSum(Tuple<DateTime, DateTime> between)
        {
            return GetRangeSum(between, _creditOp);
        }

        public Pecuniam GetCurrent(DateTime dt, float rate)
        {
            if (_transactions.Count <= 0)
                return Pecuniam.Zero;

            if (Transactions.All(x => x.AtTime > dt))
                return Pecuniam.Zero;

            var prev = Transactions[0];
            var rest = Transactions.Skip(1);

            var bal = prev.Cash.Amount;
            foreach (var t in rest)
            {
                if (t.AtTime > dt)
                    break;
                var days = (t.AtTime - prev.AtTime).TotalDays;
                bal = bal.PerDiemInterest(rate, days);
                bal = bal + t.Cash.Amount;
                prev = t;
            }
            return new Pecuniam(bal);
        }

        public Pecuniam GetCurrent(DateTime dt, Dictionary<DateTime, float> variableRate)
        {
            if (variableRate == null || variableRate.Keys.Count <= 0)
                throw new ArgumentNullException(nameof(variableRate));

            //get very first recorded transaction
            var oldestTransaction = Transactions.FirstOrDefault();
            if (oldestTransaction == null)
                return Pecuniam.Zero;

            var bal = Pecuniam.Zero;

            //set first transaction as the lower bounds of the current time-frame
            var prevVdt = oldestTransaction.AtTime;

            foreach (var vdt in variableRate.Keys)
            {
                //the dictionary is the date the rate ended, not began
                if (DateTime.Compare(prevVdt, dt) > 0)
                    continue;

                var currVdt = vdt;

                //this rate was only applicable to this many days
                var daysAtRate = (currVdt - prevVdt).TotalDays;

                //this iteration's rate-ending-date is actually beyond our query date
                if (DateTime.Compare(currVdt, dt) > 0)
                {
                    daysAtRate = (dt - prevVdt).TotalDays;
                    currVdt = dt;
                }

                //get those transactions which occured between
                var betwixtTs = GetTransactionsBetween(prevVdt, currVdt);
                betwixtTs.Sort(_comparer);

                //add in this date-ranges transactions to the running balance
                bal = betwixtTs.Aggregate(bal, (current, bts) => current + bts.Cash);

                //get the balance plus iterest for the number of days this rate was in effect
                bal = new Pecuniam(bal.Amount.PerDiemInterest(variableRate[vdt], daysAtRate));

                //save where we ended this time for next iteration
                prevVdt = currVdt;
            }

            return bal;
        }

        public List<ITransaction> GetTransactionsBetween(DateTime from, DateTime to, bool includeThoseOnToDate = false)
        {
            if (includeThoseOnToDate)
            {
                return Transactions.Where(
                    x => DateTime.Compare(x.AtTime, from) >= 0 && DateTime.Compare(x.AtTime, to) <= 0)
                    .ToList();
            }
            return Transactions.Where(
                x => DateTime.Compare(x.AtTime, from) >= 0 && DateTime.Compare(x.AtTime, to) < 0)
                .ToList();
        }

        protected internal Pecuniam GetRangeSum(Tuple<DateTime, DateTime> between, Func<Decimal, bool> op)
        {
            var ts = Transactions;
            if (ts.Count <= 0)
                return Pecuniam.Zero;

            if (between == null)
                throw new ArgumentNullException(nameof(between));

            if (between.Item1.Equals(between.Item2))
                throw new ItsDeadJim("The calculation requires a date range.");

            var olderDate = DateTime.Compare(between.Item1, between.Item2) < 0 ? between.Item1 : between.Item2;
            var newerDate = DateTime.Compare(between.Item2, between.Item1) > 0 ? between.Item2 : between.Item1;

            var paymentsInRange =
                ts.Where(
                    x =>
                        DateTime.Compare(x.AtTime, olderDate) >= 0 && DateTime.Compare(x.AtTime, newerDate) <= 0 &&
                        op(x.Cash.Amount))
                    .ToList();

            if (paymentsInRange.Count == 0)
                return Pecuniam.Zero;

            var sumPayments = paymentsInRange.Select(x => x.Cash.Amount).Sum();
            return new Pecuniam(sumPayments);
        }
        #endregion
    }
}
