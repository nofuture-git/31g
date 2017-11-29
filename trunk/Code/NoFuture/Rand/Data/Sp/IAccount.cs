﻿using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents a individual finacial agreement in time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAccount<T> : IAsset, ITempore
    {
        T Id { get; }
    }
}