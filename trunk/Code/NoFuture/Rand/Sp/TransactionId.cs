using System;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    public abstract class TransactionId : ITransactionId
    {
        protected internal TransactionId(DateTime atTime, Guid accountId) :this(Guid.NewGuid(), atTime, accountId)
        {
        }

        private TransactionId(Guid uniqueId, DateTime atTime, Guid accountId)
        {
            UniqueId = uniqueId;
            AtTime = atTime;
            AccountId = accountId;
        }

        public Guid AccountId { get; }
        public Guid UniqueId { get; }
        public DateTime AtTime { get; }
        public ITransactionId Trace { get; protected internal set; }
    }

    public class TraceTransactionId : ITransactionId
    {
        public TraceTransactionId(Guid uniqueId, Guid accountId, DateTime atTime)
        {
            AccountId = accountId;
            UniqueId = uniqueId;
            AtTime = atTime;
        }

        public Guid AccountId { get; }
        public Guid UniqueId { get; }
        public DateTime AtTime { get; }
        public ITransactionId Trace { get; protected internal set; }
        public override string ToString()
        {
            return new Tuple<Guid, Guid, DateTime>(UniqueId, AccountId, AtTime).ToString();
        }
    }
}
