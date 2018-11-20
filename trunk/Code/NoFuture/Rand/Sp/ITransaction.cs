using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a single one-time exchange at an exact moment in-time.
    /// </summary>
    public interface ITransaction : ITransactionId
    {
        Pecuniam Cash { get; }
        IVoca Description { get; }
        ITransaction GetInverse();
        ITransaction Clone();
    }
}