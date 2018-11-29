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
    public class Journal : VocaBase
    {
        public Journal(){ }
        public Journal(string name) : base(name) { }
        public Journal(string name, string group) : base(name, group) { }

        protected internal SortedSet<ITransaction> Transactions { get; } = new SortedSet<ITransaction>(new TransactionComparer());

        protected void AddTransaction(ITransaction t)
        {
            if (t != null)
            {
                Transactions.Add(t);
            }
        }

        protected internal IComparer<ITransaction> Comparer { get; } = new TransactionComparer();

        public bool IsEmpty => Transactions.Count <= 0;

        public ITransaction FirstTransaction => Transactions.FirstOrDefault();

        public ITransaction LastTransaction => Transactions.LastOrDefault();

        public int TransactionCount => Transactions.Count;

        public double DaysPerYear { get; set; } = Shared.Core.Constants.DBL_TROPICAL_YEAR;

        public virtual Guid AddNegativeValue(DateTime dt, Pecuniam amnt, IVoca note = null, ITransactionId trace = null)
        {
            if (amnt == null)
                return Guid.Empty;
            if (amnt == Pecuniam.Zero)
                return Guid.Empty;
            while (Transactions.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
            {
                dt = dt.AddTicks(1L);
            }
            var t = new Transaction(dt, amnt.GetNeg(), note) { Trace = trace };
            AddTransaction(t);
            return t.UniqueId;
        }

        public virtual Guid AddPositiveValue(DateTime dt, Pecuniam amnt, IVoca note = null, ITransactionId trace = null)
        {
            if (amnt == null)
                return Guid.Empty;
            if (amnt == Pecuniam.Zero)
                return Guid.Empty;
            while (Transactions.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
            {
                dt = dt.AddTicks(1L);
            }

            var t = new Transaction(dt, amnt.GetAbs(), note) {Trace = trace};
            AddTransaction(t);
            return t.UniqueId;
        }
    }
}