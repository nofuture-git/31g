using System;
using NoFuture.Rand.Core;

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
        IVoca Description { get; }
    }
}