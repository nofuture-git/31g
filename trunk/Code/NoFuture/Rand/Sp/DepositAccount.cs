using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Cc;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Base type for a depository account held at a commercial bank.
    /// </summary>
    [Serializable]
    public abstract class DepositAccount : NamedReceivable, IAccount<Identifier>
    {
        #region ctor

        protected DepositAccount(DateTime dateOpenned) : base(dateOpenned)
        {
            DueFrequency = TimeSpan.Zero;
            FormOfCredit = Enums.FormOfCredit.None;
        }

        #endregion

        #region properties

        public bool IsJointAcct { get; set; }
        public Identifier Id { get; set; }
        public virtual Identifier RoutingNumber { get; set; }

        #endregion

        #region methods

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

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, 0.0F);
        }

        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var d = Balance.GetCurrent(dt, 0.0F);
            return d < Pecuniam.Zero ? d.GetAbs() : Pecuniam.Zero;
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
        /// Moves <see cref="amt"/> out of <see cref="fromAccount"/> to <see cref="toAccount"/>.
        /// When the amount exceeds what is available it will be divided in half continously until
        /// it fits or goes down to one penny.
        /// </summary>
        /// <param name="fromAccount">
        /// The account which will incur a decrease in cash (a.k.a. withdraw, credit, etc.)
        /// </param>
        /// <param name="toAccount">
        /// The account which will incur a increase in case (a.k.a. deposit, debit, etc.)
        /// </param>
        /// <param name="amt">
        /// Required, must be a value other than zero
        /// </param>
        /// <param name="dt">
        /// Optional, will default to the current utc time
        /// </param>
        public static void TransferFundsInBankAccounts<T>(IAccount<T> fromAccount, IAccount<T> toAccount,
            Pecuniam amt, DateTime? dt = null)
        {
            if (fromAccount == null || toAccount == null || amt == null || amt == Pecuniam.Zero)
                return;
            var dtt = dt.GetValueOrDefault(DateTime.UtcNow);
            if (fromAccount.Inception < dtt
                || toAccount.Inception < dtt)
                return;
            amt = amt.GetAbs();

            while (fromAccount.Value < amt)
            {
                amt = amt / 2.ToPecuniam();
                if (amt.Amount < 0.01M)
                    break;
            }

            toAccount.Balance.AddPositiveValue(fromAccount.Balance, amt, dtt);;
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

        #endregion
    }
}