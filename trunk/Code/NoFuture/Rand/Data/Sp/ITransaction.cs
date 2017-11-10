using System;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents a single one-time currency exchange 
    /// </summary>
    public interface ITransaction
    {
        Guid UniqueId { get; }
        DateTime AtTime { get; }
        Pecuniam Cash { get; }
        Pecuniam Fee { get; }
        string Description { get; }
    }
}