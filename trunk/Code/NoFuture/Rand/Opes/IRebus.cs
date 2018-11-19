using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;

namespace NoFuture.Rand.Opes
{
    /// <summary>
    /// Represents personal assets in time. 
    /// Is Latin for affairs.
    /// </summary>
    public interface IRebus : IDeinde, IObviate
    {
        /// <summary>
        /// Gets a list of assets as they were at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        NamedReceivable[] GetAssetsAt(DateTime? dt);

        /// <summary>
        /// Gets the current list of assets
        /// </summary>
        NamedReceivable[] CurrentAssets { get; }

        /// <summary>
        /// Gets the money sum of all assets for the current time
        /// </summary>
        Pecuniam TotalCurrentValue { get; }

    }
}
