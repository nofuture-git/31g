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
        public double DaysPerYear { get; set; } = Shared.Core.Constants.DBL_TROPICAL_YEAR;
        #endregion

        public Guid AddTransaction(DateTime dt, Pecuniam amnt, IVoca note = null, Pecuniam fee = null)
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