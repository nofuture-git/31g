using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="TransactionId"/>
    /// <inheritdoc cref="ITransaction"/>
    /// <summary>
    /// Single immutable money transaction
    /// </summary>
    [Serializable]
    public sealed class Transaction : TransactionId, ITransaction
    {
        private Transaction(DateTime atTime, Pecuniam amt, IVoca description = null):base(atTime, description)
        {
            Cash = amt ?? Pecuniam.Zero;
        }
        
        public Pecuniam Cash { get; }

        public TraceTransactionId GetThisAsTraceId(DateTime? atTime = null, IVoca journalName = null)
        {
            var dt = atTime ?? AtTime;

            //get copy of myself as a transaction id
            var innerTrace = new TraceTransactionId(this, dt);

            //with this, consider linked-list of trace as journal -> myself -> my-trace
            if (journalName != null && journalName.AnyNames())
            {
                innerTrace = new TraceTransactionId(this, dt, journalName)
                {
                    Trace = new TraceTransactionId(this)
                };
            }

            return innerTrace;
        }

        internal static Guid AddTransaction(ISet<ITransaction> items, DateTime dt, Pecuniam amount,
            IVoca note = null, ITransactionId trace = null)
        {
            if(items == null)
                throw new ArgumentNullException(nameof(items));
            var t = new Transaction(dt, amount, note) { Trace = trace };
            items.Add(t);
            return t.UniqueId;
        }

        public override bool Equals(object obj)
        {
            var t = obj as Transaction;
            return t != null && UniqueId.Equals(t.UniqueId);
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join("\t", UniqueId, $"{AtTime:yyyy-MM-dd HH:mm:ss.ffff}", $"{Cash.Amount:0.00}", Description);
        }

    }
}
