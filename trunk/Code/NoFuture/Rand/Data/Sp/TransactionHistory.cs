using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Data.Sp
{
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

        #endregion
        public void AddTransaction(DateTime dt, Pecuniam amnt, string note = null)
        {
            if (amnt == Pecuniam.Zero)
                return;
            while (_transactions.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
            {
                dt = dt.AddMilliseconds(10);
            }
            var t = new Transaction(dt, amnt, note);
            _transactions.Add(t);
        }
    }
}
