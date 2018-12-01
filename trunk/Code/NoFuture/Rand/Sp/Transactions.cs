using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="ITransactionable"/>
    /// <summary>
    /// Represent an ordered set of <see cref="T:NoFuture.Rand.Sp.Transaction" /> in time.
    /// </summary>
    [Serializable]
    public abstract class Transactions : VocaBase, ITransactionable
    {
        protected Transactions(){ }
        protected Transactions(string name) : base(name) { }
        protected Transactions(string name, string group) : base(name, group) { }

        protected internal SortedSet<ITransaction> DataSet { get; } = new SortedSet<ITransaction>(new TransactionComparer());

        protected internal IComparer<ITransaction> Comparer { get; } = new TransactionComparer();

        public bool IsEmpty => DataSet.Count <= 0;

        public ITransaction FirstTransaction => DataSet.FirstOrDefault();

        public ITransaction LastTransaction => DataSet.LastOrDefault();

        public int TransactionCount => DataSet.Count;

        public double DaysPerYear { get; set; } = Shared.Core.Constants.DBL_TROPICAL_YEAR;

        public virtual Guid AddNegativeValue(DateTime dt, Pecuniam amount, IVoca note = null, ITransactionId trace = null)
        {
            if (amount == null)
                return Guid.Empty;
            if (amount == Pecuniam.Zero)
                return Guid.Empty;
            while (DataSet.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
            {
                dt = dt.AddTicks(1L);
            }
            return Transaction.AddTransaction(DataSet, dt, amount.GetNeg(), note, trace);
        }

        public virtual Guid AddPositiveValue(DateTime dt, Pecuniam amount, IVoca note = null, ITransactionId trace = null)
        {
            if (amount == null)
                return Guid.Empty;
            if (amount == Pecuniam.Zero)
                return Guid.Empty;
            while (DataSet.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
            {
                dt = dt.AddTicks(1L);
            }

            return Transaction.AddTransaction(DataSet, dt, amount.GetAbs(), note, trace);
        }
    }
}