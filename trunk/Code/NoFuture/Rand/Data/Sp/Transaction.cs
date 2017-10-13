using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Single immutable money transaction
    /// </summary>
    [Serializable]
    public class Transaction : ITransaction
    {
        #region ctor
        public Transaction(DateTime atTime, Pecuniam amt, string description = null)
        {
            AtTime = atTime;
            Cash = amt;
            Description = description;
        }
        public Transaction(DateTime atTime, Pecuniam amt, Pecuniam fee, string description = null)
        {
            AtTime = atTime;
            Cash = amt;
            Description = description;
            Fee = fee;
        }
        #endregion

        #region properties
        public Guid UniqueId { get; } = Guid.NewGuid();
        public DateTime AtTime { get; }
        public Pecuniam Cash { get; }
        public Pecuniam Fee { get; }
        public string Description { get; }
        #endregion

        #region overrides
        public override bool Equals(object obj)
        {
            if (Equals(obj, null))
                return false;
            var t = obj as Transaction;
            if (t == null)
                return false;
            return UniqueId.Equals(t.UniqueId);
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join("\t", UniqueId, $"{AtTime:yyyy-MM-dd HH:mm:ss.ffff}", $"{Cash.Amount:0.00}", Description);
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
            if (x == null)
                return 1;
            if (y == null)
                return -1;
            return DateTime.Compare(x.AtTime, y.AtTime);
        }
    }

    [Serializable]
    public class TransactionHistory
    {
        #region fields
        private readonly List<ITransaction> _transactions = new List<ITransaction>();
        #endregion

        #region properties
        protected internal List<ITransaction> Transactions
        {
            get
            {
                _transactions.Sort(Comparer);
                return _transactions;
            }
        }

        protected internal IComparer<ITransaction> Comparer { get; } = new TransactionComparer();

        public bool IsEmpty => _transactions.Count <= 0;

        public ITransaction FirstTransaction => Transactions.FirstOrDefault();
        public ITransaction LastTransaction => Transactions.LastOrDefault();
        public int TransactionCount => Transactions.Count;

        #endregion
        public Guid AddTransaction(DateTime dt, Pecuniam amnt, Pecuniam fee = null, string note = null)
        {
            if (amnt == null)
                return Guid.Empty;
            if (amnt == Pecuniam.Zero)
                return Guid.Empty;
            while (_transactions.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
            {
                dt = dt.AddMilliseconds(10);
            }
            fee = fee ?? Pecuniam.Zero;
            var t = new Transaction(dt, amnt, fee, note);
            _transactions.Add(t);
            return t.UniqueId;
        }
    }
}
