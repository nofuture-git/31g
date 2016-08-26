using System;

namespace NoFuture.Rand.Data.Sp
{
    public interface IReceivable : IAsset
    {
        string Description { get; set; }
        ITradeLine TradeLine { get; }

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
        /// Get the current balance for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pecuniam GetBalance(DateTime dt);
    }

}
