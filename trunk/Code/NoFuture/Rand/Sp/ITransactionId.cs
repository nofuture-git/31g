using System;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represents the identity of a single transaction
    /// </summary>
    public interface ITransactionId
    {
        /// <summary>
        /// The id the the ledger currently containing this transaction
        /// </summary>
        Guid LedgerId { get; }

        /// <summary>
        /// The unique id of this transaction
        /// </summary>
        Guid UniqueId { get; }

        /// <summary>
        /// The time at which the transaction occured in time
        /// </summary>
        DateTime AtTime { get; }

        /// <summary>
        /// A record of where this transaction has been
        /// </summary>
        ITransactionId Trace { get; }

    }
}
