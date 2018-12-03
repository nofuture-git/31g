using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="IValoresTempus" />
    /// <summary>
    /// Represents a individual finacial agreement in time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAccount<out T> : IValoresTempus, IVoca
    {
        T Id { get; }

        KindsOfAccounts? AccountType { get; }

        IBalance Balance { get; }

        /// <summary>
        /// In opposite form the account operates like a liability the default is false which
        /// operates like an asset (in the Accounting sense).  
        /// This is also known as debit-balance\credit-balance
        /// </summary>
        bool IsOppositeForm { get; }

        /// <summary>
        /// In normal form will record debits as an increase in cash, in <see cref="IsOppositeForm"/>
        /// a debit will reduce the cash of the account
        /// </summary>
        IAccount<T> Debit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null);

        /// <summary>
        /// In normal form will record credits as a decrease in cash, in <see cref="IsOppositeForm"/>
        /// a credit will increase the cash of the account
        /// </summary>
        IAccount<T> Credit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null);

        /// <summary>
        /// Method to test for a transaction which is matched to the given predicate
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        bool AnyTransaction(Predicate<ITransactionId> filter);
    }
}