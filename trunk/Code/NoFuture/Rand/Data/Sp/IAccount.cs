using System;

namespace NoFuture.Rand.Data.Sp
{
    public interface IAccount : IAsset
    {
        Identifier Id { get; }
        DateTime OpenDate { get; }
        DateTime? ClosedDate { get; set; }
    }
}
