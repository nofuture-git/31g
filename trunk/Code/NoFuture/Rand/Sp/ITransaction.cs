using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represents a single one-time currency exchange 
    /// </summary>
    public interface ITransaction
    {
        Guid LedgerId { get; }
        Guid UniqueId { get; }
        DateTime AtTime { get; }
        Pecuniam Cash { get; }
        IVoca Description { get; }
        ITransaction GetInverse();
    }
}