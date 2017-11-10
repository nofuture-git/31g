using System;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents a individual finacial agreement in time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAccount<T> : IAsset
    {
        T Id { get; }
        DateTime Inception { get; }
        DateTime? Terminus { get; set; }
    }
}