using System;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// A general type for duality of financial transactions (e.g. Buy\Sell, Long\Short, CashIn\CashOut),
    /// by removing control over a value&apos;s sign (positive\negative) and forcing the caller 
    /// to explictly choose one or the other.
    /// </summary>
    public interface ITransactionable
    {
        void AddNegativeValue(DateTime dt, Pecuniam amt, IMereo note = null, Pecuniam fee = null);
        bool AddPositiveValue(DateTime dt, Pecuniam amt, IMereo note = null, Pecuniam fee = null);
    }
}