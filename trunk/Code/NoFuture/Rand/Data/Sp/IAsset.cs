using System;

namespace NoFuture.Rand.Data.Sp
{
    public interface IAsset
    {
        Pecuniam Value { get; }
        /// <summary>
        /// Get the loans status for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        AccountStatus GetStatus(DateTime dt);
    }
}
