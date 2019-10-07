using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a currency value in-time in 
    /// the form of a push-only stack of transactions
    /// </summary>
    public interface IBalance : ITransactionable
    {
        bool IsEmpty { get; }

        ITransaction FirstTransaction { get; }

        ITransaction LastTransaction { get; }

        int TransactionCount { get; }

        /// <summary>
        /// Gets transactions which occured on or after <see cref="from"/> up to the <see cref="to"/>
        /// </summary>
        /// <param name="from">
        /// Optional, leave null to default to first transaction.
        /// Transactions which occured exactly on this date WILL be included in the results.
        /// </param>
        /// <param name="to">
        /// Optional, leave null to default to last transaction
        /// Transactions which occured exactly on this date will not be included unless <see cref="includeThoseOnToDate"/>
        /// is set to true.
        /// </param>
        /// <param name="includeThoseOnToDate"></param>
        /// <returns></returns>
        List<ITransaction> GetTransactions(DateTime? from = null, DateTime? to = null, bool includeThoseOnToDate = false);

        /// <summary>
        /// Same as <see cref="GetTransactions"/> except exclusive to positive values
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="includeThoseOnToDate"></param>
        /// <returns></returns>
        List<ITransaction> GetDebits(DateTime? from = null, DateTime? to = null, bool includeThoseOnToDate = true);

        /// <summary>
        /// Same as <see cref="GetTransactions"/> except exclusive to negative values
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="includeThoseOnToDate"></param>
        /// <returns></returns>
        List<ITransaction> GetCredits(DateTime? from = null, DateTime? to = null, bool includeThoseOnToDate = true);

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
        /// The dictionary keys are the dates the rate values end on, NOT begin on.
        /// </param>
        /// <returns></returns>
        Pecuniam GetCurrent(DateTime dt, Dictionary<DateTime, float> variableRate);

        /// <summary>
        /// The number of days in a year used to calc per-diem interest.
        /// This is a double to reflect that a tropical-year is not a whole number.
        /// </summary>
        double DaysPerYear { get; set; }

        /// <summary>
        /// Gets ordered table of dates-to-cash sum for all the days between <see cref="from"/> and <see cref="to"/>
        /// </summary>
        /// <param name="from">Optional, will use first transaction&apos;s date if null</param>
        /// <param name="to">Optional, will use last transaction&apos;s date if null</param>
        /// <returns>
        /// A sorted dictionary of the date-only to the sum of that day - dates with no transactions will be omitted.
        /// </returns>
        SortedDictionary<DateTime, Pecuniam> GetSumPerDay(DateTime? from = null, DateTime? to = null);

        /// <summary>
        /// Method to test for a transaction which is matched to the given predicate
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        bool AnyTransaction(Predicate<ITransactionId> filter);
    }
}