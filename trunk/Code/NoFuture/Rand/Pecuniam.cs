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
        /// Returns a negative value being the sum of all payments between dates in <see cref="between"/>
        /// </summary>
        /// <param name="between"></param>
        /// <returns></returns>
        Pecuniam GetPaymentSum(Tuple<DateTime, DateTime> between);

        /// <summary>
        /// Gets the current balance up to the <see cref="dt"/> for the
        /// given rate of <see cref="rate"/>.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        Pecuniam GetCurrent(DateTime dt, float rate);

        Pecuniam GetCurrent(DateTime dt, Func<DateTime, float> variableRate);
    }

    [Serializable]
    public class Balance : IBalance
    {
        #region fields
        private readonly List<Transaction>  _transactions = new List<Transaction>();
        private readonly IComparer<Transaction> _comparer = new TransactionComparer();
        #endregion

        #region methods

        public List<Transaction> Transactions
        {
            get
            {
                _transactions.Sort(_comparer);
                return _transactions;
            }
        }

        public Pecuniam GetPaymentSum(Tuple<DateTime, DateTime> between)
        {
            var ts = Transactions;
            if (ts.Count <= 0)
                return new Pecuniam(0);

            if (between == null)
                throw new ArgumentNullException("between");

            if(between.Item1.Equals(between.Item2))
                throw new ItsDeadJim("The calculation requires a date range.");

            var olderDate = DateTime.Compare(between.Item1, between.Item2) < 0 ? between.Item1 : between.Item2;
            var newerDate = DateTime.Compare(between.Item1, between.Item2) > 0 ? between.Item2 : between.Item1;

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
            if(_transactions.All(x => DateTime.Compare(x.AtTime, dt) > 0))
                return new Pecuniam(0);

            var ts = Transactions;
            var prev = ts[0];
            var rest = ts.Skip(1);

            var bal = prev.Cash.Amount;
            foreach (var t in rest)
            {
                if(DateTime.Compare(t.AtTime, dt) > 0)
                    break;
                var days = (t.AtTime - prev.AtTime).TotalDays;
                bal = bal.PerDiemInterest(rate, days);
                bal = bal + t.Cash.Amount;
                prev = t;
            }
            return new Pecuniam(bal);
        }

        public Pecuniam GetCurrent(DateTime dt, Func<DateTime, float> variableRate)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
