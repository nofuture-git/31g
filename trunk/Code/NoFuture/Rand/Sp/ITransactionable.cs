﻿using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// A general type for duality of financial transactions (e.g. Buy\Sell, Long\Short, CashIn\CashOut, Credit\Debit),
    /// by removing control over a value&apos;s sign (positive\nnegative) and forcing the caller 
    /// to explicitly choose one or the other.
    /// </summary>
    public interface ITransactionable
    {
        /// <summary>
        /// Will consider <see cref="amount"/> as a negative value.
        /// </summary>
        Guid AddNegativeValue(DateTime dt, Pecuniam amount, IVoca note = null, ITransactionId trace = null);

        /// <summary>
        /// Will consider <see cref="amount"/> as a positive value.
        /// </summary>
        Guid AddPositiveValue(DateTime dt, Pecuniam amount, IVoca note = null, ITransactionId trace = null);
    }
}