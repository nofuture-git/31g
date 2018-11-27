using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// A general type for duality of financial transactions (e.g. Buy\Sell, Long\Short, CashIn\CashOut, Credit\Debit),
    /// by removing control over a value&apos;s sign (positive\negative) and forcing the caller 
    /// to explictly choose one or the other.
    /// </summary>
    public interface ITransactionable
    {
        /// <summary>
        /// Will consider <see cref="amt"/> as a negative value.
        /// </summary>
        Guid AddNegativeValue(DateTime dt, Pecuniam amt, IVoca note = null, ITransactionId trace = null);

        /// <summary>
        /// Will consider <see cref="amt"/> as a positive value.
        /// </summary>
        Guid AddPositiveValue(DateTime dt, Pecuniam amt, IVoca note = null, ITransactionId trace = null);

        /// <summary>
        /// Takes cash from <see cref="source"/> and puts it into this instance.
        /// </summary>
        Guid AddPositiveValue(ITransactionable source, Pecuniam amount, DateTime? atTime = null, IVoca description = null);

        /// <summary>
        /// Takes cash from this instance and puts it into <see cref="source"/>
        /// </summary>
        Guid AddNegativeValue(ITransactionable source, Pecuniam amount, DateTime? atTime = null, IVoca description = null);
    }
}