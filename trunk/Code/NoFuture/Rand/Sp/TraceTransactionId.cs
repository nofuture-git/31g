using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// A type specific only to audit.
    /// </summary>
    public sealed class TraceTransactionId : ITransactionId
    {
        public TraceTransactionId(ITransaction transactionId, DateTime? atTime = null, IVoca description = null)
        {
            if(transactionId == null)
                throw new ArgumentNullException(nameof(transactionId));

            Description = description ?? transactionId.Description;
            AtTime = atTime ?? transactionId.AtTime;
            UniqueId = transactionId.UniqueId;
            Trace = transactionId.Trace;
        }

        public IVoca Description { get; internal set; }
        public Guid UniqueId { get; }
        public DateTime AtTime { get; }
        public ITransactionId Trace { get; internal set; }
        public override string ToString()
        {
            return new Tuple<Guid, string, DateTime>(UniqueId, Description?.Name, AtTime).ToString();
        }
    }
}