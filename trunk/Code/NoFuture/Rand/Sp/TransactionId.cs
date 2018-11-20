using System;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    public class TransactionId : ITransactionId
    {
        internal TransactionId(DateTime atTime, Guid ledgerId)
        {
            UniqueId = Guid.NewGuid();
            AtTime = atTime;
            LedgerId = ledgerId;
        }

        internal TransactionId(DateTime atTime, Guid ledgerId, Guid fromLedgerId,
            ITransactionId history) : this(atTime, ledgerId)
        {
            Trace = history;
            FromLedgerId = fromLedgerId;
        }

        public Guid FromLedgerId { get; }
        public Guid LedgerId { get; }
        public Guid UniqueId { get; }
        public DateTime AtTime { get; }
        public ITransactionId Trace { get; }
        public virtual void PushTrace(Guid fromLedgerId, Guid toLedgerId, DateTime? atTime = null)
        {
            throw new NotImplementedException();
        }
    }
}
