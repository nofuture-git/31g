using System;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents something of value which may be sold 
    /// on the open market for cash
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// The current value of this asset
        /// </summary>
        Pecuniam Value { get; }

        /// <summary>
        /// Get the status for the given asset at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        SpStatus GetStatus(DateTime? dt);

        /// <summary>
        /// Get the current balance for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pecuniam GetValueAt(DateTime dt);
    }
}