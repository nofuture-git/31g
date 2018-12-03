using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represents the identity of a single transaction
    /// </summary>
    public interface ITransactionId
    {
        /// <summary>
        /// Records the phase of a transaction in circular time.
        /// </summary>
        Enum Phase { get; }

        /// <summary>
        /// Any kind of name for this transaction
        /// </summary>
        IVoca Description { get; }

        /// <summary>
        /// The unique id of this transaction
        /// </summary>
        Guid UniqueId { get; }

        /// <summary>
        /// Records the date and time of a transaction in linear time.
        /// </summary>
        DateTime AtTime { get; }

        /// <summary>
        /// A record of where this transaction has been
        /// </summary>
        ITransactionId Trace { get; }

    }
}
