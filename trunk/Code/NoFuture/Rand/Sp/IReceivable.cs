using System;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="IAsset" />
    /// <inheritdoc cref="ITradeLine" />
    /// <summary>
    /// Represents money owed by debtors
    /// </summary>
    public interface IReceivable : IAsset, ITradeLine
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
        /// Represents the expected value of the given receivable
        /// </summary>
        IMereo Expectation { get; }

        /// <summary>
        /// Get the status, if applicable, of the receivable at the current time.
        /// </summary>
        SpStatus? CurrentStatus { get; }
    }
}