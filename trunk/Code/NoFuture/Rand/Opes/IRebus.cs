using System;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// Represents personal assets in time. 
    /// Is Latin for affairs.
    /// </summary>
    public interface IRebus : IDeinde
    {
        /// <summary>
        /// Gets a list of assets as they were at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pondus[] GetAssetsAt(DateTime? dt);

        /// <summary>
        /// Gets the current list of assets
        /// </summary>
        Pondus[] CurrentAssets { get; }

        /// <summary>
        /// Gets the money sum of all assets for the current time
        /// </summary>
        Pecuniam TotalCurrentExpectedValue { get; }

    }
}
