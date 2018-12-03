using System;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="IValoresTempus" />
    /// <inheritdoc cref="ITradeLine" />
    /// <summary>
    /// Represents an asset as participatory set of 
    /// money records through time and the sense-of-status of said 
    /// record-set.
    /// </summary>
    public interface IReceivable : ITradeLine
    {
        /// <summary>
        /// Determins the deliquency for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        PastDue? GetDelinquency(DateTime dt);

        /// <summary>
        /// Calc's the minimum payment for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pecuniam GetMinPayment(DateTime dt);

        /// <summary>
        /// Get the status, if applicable, of the receivable at the current time.
        /// </summary>
        SpStatus? CurrentStatus { get; }

        /// <summary>
        /// Get the status for the given asset at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        SpStatus GetStatus(DateTime? dt);

    }
}