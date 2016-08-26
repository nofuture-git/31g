using System;

namespace NoFuture.Rand.Data.Sp
{
    public interface ITransaction
    {
        Guid UniqueId { get; }
        DateTime AtTime { get; }
        Pecuniam Cash { get; }
        Pecuniam Fee { get; }
        string Description { get; }
    }
}
