using System;
using System.Collections.Generic;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Data.Sp //Sequere pecuniam
{
    /// <summary>
    /// Represents something of value which may be sold 
    /// on the open market for cash
    /// </summary>
    public interface IAsset
    {
        Pecuniam CurrentValue { get; }
        /// <summary>
        /// Get the loans status for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        SpStatus GetStatus(DateTime? dt);

        /// <summary>
        /// Get the current balance for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pecuniam GetValueAt(DateTime dt);
    }

    /// <summary>
    /// Represents a individual finacial agreement in time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAccount<T> : IAsset
    {
        T Id { get; }
        DateTime Inception { get; }
        DateTime? Terminus { get; set; }
    }

    /// <summary>
    /// Represents the item reported to a Credit Bureau
    /// </summary>
    public interface ITradeLine
    {
        FormOfCredit FormOfCredit { get; set; }
        Pecuniam CreditLimit { get; set; }
        IBalance Balance { get; }
        TimeSpan DueFrequency { get; set; }
        DateTime OpennedDate { get; }
        TradelineClosure? Closure { get; set; }
    }

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
        /// <param name="fee"></param>
        /// <param name="note"></param>
        Guid AddTransaction(DateTime dt, Pecuniam amnt, Pecuniam fee = null, string note = null);

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
        List<ITransaction> GetTransactionsBetween(DateTime from, DateTime to, bool includeThoseOnToDate = false);

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

    /// <summary>
    /// Represents the std properites from a card-issuer
    /// </summary>
    public interface ICreditCard
    {
        CreditCardNumber Number { get; }
        DateTime ExpDate { get; }
        string CardHolderName { get; }
        string Cvv { get; }
        DateTime CardHolderSince { get; }
    }

    /// <summary>
    /// Represent finacial loan from a money-lending agent
    /// </summary>
    public interface ILoan
    {
        /// <summary>
        /// The rate used to calc the minimum payment.
        /// </summary>
        float MinPaymentRate { get; set; }

        IFirm Lender { get; set; }
    }

    /// <summary>
    /// Represents money owed by debtors
    /// </summary>
    public interface IReceivable : IAsset
    {
        string Description { get; set; }
        ITradeLine TradeLine { get; }

        /// <summary>
        /// Determins the deliquency for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        PastDue? GetDelinquency(DateTime dt);

        /// <summary>
        /// Calc's the minimum payment for the given <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        Pecuniam GetMinPayment(DateTime dt);
    }

    /// <summary>
    /// Represents a single one-time currency exchange 
    /// </summary>
    public interface ITransaction
    {
        Guid UniqueId { get; }
        DateTime AtTime { get; }
        Pecuniam Cash { get; }
        Pecuniam Fee { get; }
        string Description { get; }
    }

    /// <summary>
    /// A general type for duality of financial transactions (e.g. Buy\Sell, Long\Short, CashIn\CashOut)
    /// </summary>
    public interface ITransactionable
    {
        void Push(DateTime dt, Pecuniam amt, Pecuniam fee = null, string note = null);
        bool Pop(DateTime dt, Pecuniam amt, Pecuniam fee = null, string note = null);
    }

    /// <summary>
    /// Any type which has a count-of and an identifier
    /// </summary>
    /// <remarks>Latin for 'be counted'</remarks>
    public interface INumera
    {
        Decimal Amount { get; }
        Identifier Id { get; }
    }
}
