using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represent an ordered set of <see cref="Transaction"/> in time.
    /// </summary>
    [Serializable]
    public class Ledger
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

        protected void AddTransaction(ITransaction t)
        {
            if (t != null)
            {
                _transactions.Add(t);
            }
        }

        protected internal IComparer<ITransaction> Comparer { get; } = new TransactionComparer();

        public bool IsEmpty => _transactions.Count <= 0;

        public ITransaction FirstTransaction => Transactions.FirstOrDefault();
        public ITransaction LastTransaction => Transactions.LastOrDefault();
        public int TransactionCount => Transactions.Count;
        public double DaysPerYear { get; set; } = Shared.Core.Constants.DBL_TROPICAL_YEAR;
        public Guid Id { get; } = Guid.NewGuid();

        #endregion

        public Guid AddNegativeValue(DateTime dt, Pecuniam amnt, IVoca note = null, ITransactionId trace = null)
        {
            if (amnt == null)
                return Guid.Empty;
            if (amnt == Pecuniam.Zero)
                return Guid.Empty;
            while (_transactions.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
            {
                dt = dt.AddTicks(1L);
            }
            var t = new Transaction(dt, amnt.GetNeg(), Id, note) { Trace = trace };
            AddTransaction(t);
            return t.UniqueId;
        }

        public Guid AddPositiveValue(DateTime dt, Pecuniam amnt, IVoca note = null, ITransactionId trace = null)
        {
            if (amnt == null)
                return Guid.Empty;
            if (amnt == Pecuniam.Zero)
                return Guid.Empty;
            while (_transactions.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
            {
                dt = dt.AddTicks(1L);
            }

            var t = new Transaction(dt, amnt.GetAbs(), Id, note) {Trace = trace};
            AddTransaction(t);
            return t.UniqueId;
        }
    }
}