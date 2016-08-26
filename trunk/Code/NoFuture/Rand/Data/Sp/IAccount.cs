using System;

namespace NoFuture.Rand.Data.Sp
{
    public interface IAccount<T> : IAsset
    {
        T Id { get; }
        DateTime OpenDate { get; }
        DateTime? ClosedDate { get; set; }
    }
}
