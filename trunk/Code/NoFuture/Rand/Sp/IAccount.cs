using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="IValoresTempus" />
    /// <inheritdoc cref="IVoca" />
    /// <summary>
    /// An uniquely identified balance of some asset, liability or owner&apos;s equity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAccount<out T> : IValoresTempus, IVoca
    {
        /// <summary>
        /// The associated identity of the given <see cref="Balance"/>
        /// </summary>
        T Id { get; }

        /// <summary>
        /// Where the given account appears in the Basic Accounting Equation
        /// </summary>
        KindsOfAccounts? AccountType { get; }

        /// <summary>
        /// The mathematical center of an account in terms of actual history.
        /// </summary>
        IBalance Balance { get; }

        /// <summary>
        /// In opposite form the account operates like a liability the default is false which
        /// operates like an asset (in the Accounting sense).  
        /// This is also known as debit-balance\credit-balance
        /// </summary>
        bool IsOppositeForm { get; }

        /// <summary>
        /// The Left side entries of a T-Account book system (abbrev. Dr).
        /// </summary>
        IAccount<T> Debit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null);

        /// <summary>
        /// The Right side entries of a T-account book system (abbrev. Cr)
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