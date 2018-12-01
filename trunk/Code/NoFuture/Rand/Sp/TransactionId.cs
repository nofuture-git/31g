using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    public abstract class TransactionId : ITransactionId
    {
        internal TransactionId(DateTime atTime, IVoca description) :this(Guid.NewGuid(), atTime, description)
        {
        }

        private TransactionId(Guid uniqueId, DateTime atTime, IVoca description)
        {
            UniqueId = uniqueId;
            AtTime = atTime;
            Description = description;
        }

        public IVoca Description { get; protected internal set; }
        public Guid UniqueId { get; }
        public DateTime AtTime { get; }
        public ITransactionId Trace { get; protected internal set; }
        public override bool Equals(object obj)
        {
            var t = obj as ITransaction;
            if (t == null)
                return false;
            return t.UniqueId == UniqueId;
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }
    }
}
