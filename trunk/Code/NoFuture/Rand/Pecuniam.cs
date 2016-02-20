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
        public Decimal Amount { get { return _amount; } }
        public CurrencyAbbrev CurrencyAbbrev { get { return _currencyAbbrev; } }
        public Pecuniam Abs { get { return new Pecuniam(Math.Abs(_amount));} }
        #endregion

        #region overrides
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var p1 = obj as Pecuniam;
            if (p1 == null)
                return false;
            return p1.Amount == Amount;
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }

        public static Pecuniam operator +(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? new Pecuniam(0);
            var pp2 = p2 ?? new Pecuniam(0);
            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount + pp2.Amount);
        }

        public static Pecuniam operator -(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? new Pecuniam(0);
            var pp2 = p2 ?? new Pecuniam(0);

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                throw new RahRowRagee("Can only add currencies from the same nation");

            return new Pecuniam(pp1.Amount - pp2.Amount);
        }

        public static bool operator ==(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? new Pecuniam(0);
            var pp2 = p2 ?? new Pecuniam(0);

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
            var pp1 = p1 ?? new Pecuniam(0);
            var pp2 = p2 ?? new Pecuniam(0);

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount > pp2.Amount;
        }

        public static bool operator <(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? new Pecuniam(0);
            var pp2 = p2 ?? new Pecuniam(0);
            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount < pp2.Amount;
        }

        public static bool operator >=(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? new Pecuniam(0);
            var pp2 = p2 ?? new Pecuniam(0);

            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount >= pp2.Amount;
        }

        public static bool operator <=(Pecuniam p1, Pecuniam p2)
        {
            var pp1 = p1 ?? new Pecuniam(0);
            var pp2 = p2 ?? new Pecuniam(0);
            if (pp1.CurrencyAbbrev != pp2.CurrencyAbbrev)
                return false;
            return pp1.Amount <= pp2.Amount;
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
        public Guid UniqueId { get { return _guid; } }
        public DateTime AtTime { get { return _atTime; } }
        public Pecuniam Cash { get { return _cash; } }
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
        #endregion
    }

    /// <summary>
    /// Sorts by <see cref="Transaction.AtTime"/>
    /// </summary>
    [Serializable]
    public class TransactionComparer : IComparer<Transaction>
    {
        public int Compare(Transaction x, Transaction y)
        {
            return DateTime.Compare(x.AtTime, y.AtTime);
        }
    }

    public interface IBalance
    {
        /// <summary>
        /// Sorted oldest (index 0) to most current (index Count - 1)
        /// </summary>
        List<Transaction> Transactions { get; }

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
        List<Transaction> GetTransactionsBetween(DateTime from, DateTime to, bool includeThoseOnToDate = false);

        /// <summary>
        /// Returns a negative value being the sum of all payments-out between dates in <see cref="between"/>
        /// </summary>
        /// <param name="between"></param>
        /// <returns></returns>
        Pecuniam GetDebitSum(Tuple<DateTime, DateTime> between);

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
        private readonly List<Transaction>  _transactions = new List<Transaction>();
        private readonly IComparer<Transaction> _comparer = new TransactionComparer();
        #endregion

        #region properties
        public List<Transaction> Transactions
        {
            get
            {
                _transactions.Sort(_comparer);
                return _transactions;
            }
        }
        #endregion

        #region methods

        public Pecuniam GetDebitSum(Tuple<DateTime, DateTime> between)
        {
            var ts = Transactions;
            if (ts.Count <= 0)
                return new Pecuniam(0);

            if (between == null)
                throw new ArgumentNullException("between");

            if(between.Item1.Equals(between.Item2))
                throw new ItsDeadJim("The calculation requires a date range.");

            var olderDate = DateTime.Compare(between.Item1, between.Item2) < 0 ? between.Item1 : between.Item2;
            var newerDate = DateTime.Compare(between.Item2, between.Item1) > 0 ? between.Item2 : between.Item1;

            var paymentsInRange =
                ts.Where(
                    x =>
                        DateTime.Compare(x.AtTime, olderDate) >= 0 && DateTime.Compare(x.AtTime, newerDate) <= 0 &&
                        x.Cash.Amount < 0)
                    .ToList();

            if(paymentsInRange.Count == 0)
                return new Pecuniam(0);

            var sumPayments = paymentsInRange.Select(x => x.Cash.Amount).Sum();
            return new Pecuniam(sumPayments);
        }

        public Pecuniam GetCurrent(DateTime dt, float rate)
        {
            if (_transactions.Count <= 0)
                return new Pecuniam(0);

            if (Transactions.All(x => DateTime.Compare(x.AtTime, dt) > 0))
                return new Pecuniam(0);

            var prev = Transactions[0];
            var rest = Transactions.Skip(1);

            var bal = prev.Cash.Amount;
            foreach (var t in rest)
            {
                if (DateTime.Compare(t.AtTime, dt) > 0)
                    break;
                var days = (t.AtTime - prev.AtTime).TotalDays;
                bal = bal.PerDiemInterest(rate, days);
                bal = bal + t.Cash.Amount;
                prev = t;
            }
            return new Pecuniam(bal);
        }

        public List<Transaction> GetTransactionsBetween(DateTime from, DateTime to, bool includeThoseOnToDate = false)
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

        public Pecuniam GetCurrent(DateTime dt, Dictionary<DateTime, float> variableRate)
        {
            if(variableRate == null || variableRate.Keys.Count <= 0)
                throw new ArgumentNullException("variableRate");

            //get very first recorded transaction
            var oldestTransaction = Transactions.FirstOrDefault();
            if(oldestTransaction == null)
                return new Pecuniam(0);

            var bal = new Pecuniam(0);

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
        #endregion
    }
}
