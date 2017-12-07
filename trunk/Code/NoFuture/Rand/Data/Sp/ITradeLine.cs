using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents the item reported to a Credit Bureau
    /// </summary>
    public interface ITradeLine : ITempore, ITransactionable
    {
        FormOfCredit FormOfCredit { get; set; }
        IBalance Balance { get; }
        TimeSpan DueFrequency { get; set; }
        ClosedCondition Closure { get; set; }
    }
}