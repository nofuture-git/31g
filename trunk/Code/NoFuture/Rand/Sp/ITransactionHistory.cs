using System;

namespace NoFuture.Rand.Sp
{
    public interface ITransactionHistory
    {
        Guid FromLedgerId { get; }
        Guid LedgerId { get; }
        DateTime AtTime { get; }
        ITransactionHistory History { get; }
    }
}
