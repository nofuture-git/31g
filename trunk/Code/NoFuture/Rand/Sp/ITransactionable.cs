using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// A general type for duality of financial transactions (e.g. Buy\Sell, Long\Short, CashIn\CashOut),
    /// by removing control over a value&apos;s sign (positive\negative) and forcing the caller 
    /// to explictly choose one or the other.
    /// </summary>
    public interface ITransactionable
    {
        Guid AddNegativeValue(DateTime dt, Pecuniam amt, IVoca note = null);
        Guid AddPositiveValue(DateTime dt, Pecuniam amt, IVoca note = null);
    }
}