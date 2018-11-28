using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a single one-time exchange at an exact moment in-time.
    /// </summary>
    public interface ITransaction : ITransactionId
    {
        Pecuniam Cash { get; }

        /// <summary>
        /// Gets a new copy of this instance with negation of <see cref="Cash"/> value.
        /// </summary>
        ITransaction GetInverse();

        /// <summary>
        /// Gets a copy of everything in this instance except the <see cref="ITransactionId.UniqueId"/>
        /// </summary>
        ITransaction Clone();

        /// <summary>
        /// Divides this transaction into two transactions where the first has a cash value of <see cref="item1Amount"/>
        /// and the other has the remainder.
        /// </summary>
        /// <param name="item1Amount">A cash amount to the nearest hunderedth</param>
        /// <param name="atTime">Optional, default to current utc time</param>
        Tuple<ITransaction, ITransaction> SplitOnAmount(Pecuniam item1Amount, DateTime? atTime = null);

        /// <summary>
        /// Divides this transaction into two transactions where 
        /// the first has a cash value of <see cref="percent"/> of the <see cref="Cash"/> and the other has the remainder.
        /// </summary>
        /// <param name="percent">A percent value in whole or floating point representation (e.g. 56.0 is the same as 0.56)</param>
        /// <param name="atTime">Optional, default to current utc time</param>
        Tuple<ITransaction, ITransaction> SplitOnPercent(double percent, DateTime? atTime = null);
    }
}