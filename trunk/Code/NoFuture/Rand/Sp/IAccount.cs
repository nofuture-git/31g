using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Represents a individual finacial agreement in time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAccount<T> : IAsset, ITempore
    {
        T Id { get; }

        /// <summary>
        /// The complete history of the tradeline.
        /// </summary>
        IBalance Balance { get; }

        /// <summary>
        /// In opposite form the account operates like a liability the default is false which
        /// operates like an asset (in the Accounting sense).
        /// </summary>
        bool IsOppositeForm { get; }

        /// <summary>
        /// In normal form will record debits as an increase in cash, in <see cref="IsOppositeForm"/>
        /// a debit will reduce the cash of the account
        /// </summary>
        Guid Debit(DateTime dt, Pecuniam amt, IVoca note = null, ITransactionId trace = null);

        /// <summary>
        /// In normal form will record credits as a decrease in cash, in <see cref="IsOppositeForm"/>
        /// a credit will increase the cash of the account
        /// </summary>
        Guid Credit(DateTime dt, Pecuniam amt, IVoca note = null, ITransactionId trace = null);

        /// <summary>
        /// In normal form will record debits as an increase in cash, in <see cref="IsOppositeForm"/>
        /// a debit will reduce the cash of the account
        /// </summary>
        Guid Debit(ITransactionable source, Pecuniam amount, DateTime? atTime = null, IVoca description = null);

        /// <summary>
        /// In normal form will record credits as a decrease in cash, in <see cref="IsOppositeForm"/>
        /// a credit will increase the cash of the account
        /// </summary>
        Guid Credit(ITransactionable source, Pecuniam amount, DateTime? atTime = null, IVoca description = null);
    }
}