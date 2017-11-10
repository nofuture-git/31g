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
        Pecuniam CurrentValue { get; }
        /// <summary>
        /// Get the loans status for the given <see cref="dt"/>
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