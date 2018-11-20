using System;

namespace NoFuture.Rand.Sp
{
    public interface ITransactionHistory
    {
        Guid FromLedger { get; }
        Guid LedgerId { get; }
        DateTime AtTime { get; }
        ITransactionHistory History { get; }
    }
}
