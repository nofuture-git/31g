﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Represents a currency value in-time in 
    /// the form of a push-only stack of transactions
    /// </summary>
    public interface IBalance
    {
        bool IsEmpty { get; }

        /// <summary>
        /// Adds the new transaction to the balance
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="amnt"></param>
        /// <param name="note"></param>
        /// <param name="fee"></param>
        Guid AddTransaction(DateTime dt, Pecuniam amnt, IVoca note = null, Pecuniam fee = null);

        /// <summary>
        /// Gets transactions which occured on or after <see cref="from"/> up to the <see cref="to"/>
        /// </summary>
        /// <param name="from">
        /// Transactions which occured exactly on this date WILL be included in the results.
        /// </param>
        /// <param name="to">
        /// Transactions which occured exactly 
        /// on this date will not be included unless <see cref="includeThoseOnToDate"/>
        /// is set to true.
        /// </param>
        /// <param name="includeThoseOnToDate"></param>
        /// <returns></returns>
        List<ITransaction> GetTransactionsBetween(DateTime? from, DateTime? to, bool includeThoseOnToDate = false);

        /// <summary>
        /// Returns a negative value being the sum of all payments-out between dates in <see cref="between"/>
        /// </summary>
        /// <param name="between"></param>
        /// <returns></returns>
        Pecuniam GetDebitSum(Tuple<DateTime, DateTime> between);

        /// <summary>
        /// Returns a positive value being the sum of all payments-in between dates in <see cref="between"/>
        /// </summary>
        /// <param name="between"></param>
        /// <returns></returns>
        Pecuniam GetCreditSum(Tuple<DateTime, DateTime> between);

        /// <summary>
        /// Gets the current balance up to the <see cref="dt"/> for the
        /// given rate of <see cref="rate"/>.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        Pecuniam GetCurrent(DateTime dt, float rate);

        /// <summary>
        /// Gets the current balance upt to the <see cref="dt"/> for the
        /// given rates in <see cref="variableRate"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="variableRate">
        /// The dictonary keys are the dates the rate values end on, NOT begin on.
        /// </param>
        /// <returns></returns>
        Pecuniam GetCurrent(DateTime dt, Dictionary<DateTime, float> variableRate);

        /// <summary>
        /// Gets the sum of fees for all transactions up-to-and-including <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pecuniam GetFees(DateTime dt);
    }
}