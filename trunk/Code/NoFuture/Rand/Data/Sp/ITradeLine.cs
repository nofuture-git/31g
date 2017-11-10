using System;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents the item reported to a Credit Bureau
    /// </summary>
    public interface ITradeLine
    {
        FormOfCredit FormOfCredit { get; set; }
        Pecuniam CreditLimit { get; set; }
        IBalance Balance { get; }
        TimeSpan DueFrequency { get; set; }
        DateTime OpennedDate { get; }
        TradelineClosure? Closure { get; set; }
    }
}