using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Cc;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// Base type for a depository account held at a commercial bank.
    /// </summary>
    [Serializable]
    public abstract class DepositAccount : Account
    {
        protected DepositAccount(DateTime dateOpenned) : base(dateOpenned)
        {
        }

        public virtual Identifier RoutingNumber { get; set; }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x?.Replace(",", "").Replace(" ", ""), txtCase);
            var acctName = Name + RoutingNumber + Id?.ValueLastFour();
            var itemData = new Dictionary<string, object>
            {
                {textFormat(acctName + "Balance"), Value.ToString()}
            };

            return itemData;
        }

        /// <summary>
        /// Helper method to put functionality in common vernacular 
        /// - is the exact same as <see cref="ITransactionable.AddNegativeValue"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="amount"></param>
        /// <param name="note"></param>
        public virtual void Withdraw(DateTime dt, Pecuniam amount, string note = null)
        {
            if(string.IsNullOrWhiteSpace(note))
                AddNegativeValue(dt,amount);
            else
                AddNegativeValue(dt, amount, new VocaBase(note));
        }

        /// <summary>
        /// Helper method to put functionality in common vernacular 
        /// - is the exact same as <see cref="ITransactionable.AddPositiveValue"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="amount"></param>
        /// <param name="note"></param>
        public virtual void Deposit(DateTime dt, Pecuniam amount, string note = null)
        {
            if (string.IsNullOrWhiteSpace(note))
                AddPositiveValue(dt, amount);
            else
                AddPositiveValue(dt, amount, new VocaBase(note));
        }

        /// <summary>
        /// Creates a new random Checking Account
        /// </summary>
        /// <param name="personName"></param>
        /// <param name="dt">Date account was openned, default to now.</param>
        /// <param name="debitPin">
        /// Optional, when present and random instance of <see cref="CheckingAccount.DebitCard"/> is created with 
        /// this as its PIN.
        /// </param>
        /// <returns></returns>
        [RandomFactory]
        public static CheckingAccount RandomCheckingAccount(IVoca personName = null, DateTime? dt = null, string debitPin = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.UtcNow);
            var accountId = new AccountId(Etx.RandomRChars(true));
            return CheckingAccount.IsPossiablePin(debitPin)
                ? new CheckingAccount(accountId, dtd,
                    new Tuple<ICreditCard, string>(CreditCard.RandomCreditCard(personName), debitPin))
                : new CheckingAccount(accountId, dtd);
        }

        /// <summary>
        /// Creates a new random Savings Account
        /// </summary>
        /// <param name="personName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        [RandomFactory]
        public static SavingsAccount RandomSavingAccount(IVoca personName = null, DateTime? dt = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.UtcNow);
            var accountId = new AccountId(Etx.RandomRChars(true));
            return new SavingsAccount(accountId, dtd);
        }
    }
}