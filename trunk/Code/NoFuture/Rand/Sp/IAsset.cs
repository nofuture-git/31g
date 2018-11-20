using System;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represents something of value which may be sold 
    /// on the open market for cash.
    /// </summary>
    /// <remarks>
    /// Considers time as a discrete set of indivisible units where
    /// a single unit is the smallest possible division of the <see cref="T:System.DateTime" /> type.
    /// An asset is participatory in-time and <see cref="P:NoFuture.Rand.Sp.IAsset.Value" /> 
    /// is, therefore, a function of time.
    /// </remarks>
    public interface IAsset
    {
        /// <summary>
        /// The current value of this asset
        /// </summary>
        Pecuniam Value { get; }

        /// <summary>
        /// Get the current value of this asset at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pecuniam GetValueAt(DateTime dt);

    }
}