﻿using System;

namespace NoFuture.Rand.Data.Sp
{
    public interface IAsset
    {
        Pecuniam CurrentMarketValue { get; }
        /// <summary>
        /// Get the loans status for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        SpStatus GetStatus(DateTime? dt);
    }
}
