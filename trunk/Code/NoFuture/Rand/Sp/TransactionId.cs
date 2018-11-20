using System;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    public abstract class TransactionId : ITransactionId
    {
        protected internal TransactionId(DateTime atTime, Guid ledgerId) :this(Guid.NewGuid(), atTime, ledgerId)
        {
        }

        private TransactionId(Guid uniqueId, DateTime atTime, Guid ledgerId)
        {
            UniqueId = uniqueId;
            AtTime = atTime;
            LedgerId = ledgerId;
        }

        public Guid LedgerId { get; }
        public Guid UniqueId { get; }
        public DateTime AtTime { get; }
        public ITransactionId Trace { get; protected internal set; }
    }

    public class TraceTransactionId : ITransactionId
    {
        public TraceTransactionId(Guid uniqueId, Guid ledgerId, DateTime atTime)
        {
            LedgerId = ledgerId;
            UniqueId = uniqueId;
            AtTime = atTime;
        }

        public Guid LedgerId { get; }
        public Guid UniqueId { get; }
        public DateTime AtTime { get; }
        public ITransactionId Trace { get; protected internal set; }
    }
}
