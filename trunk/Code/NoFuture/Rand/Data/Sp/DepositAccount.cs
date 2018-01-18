﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// Base type for a depository account held at a commercial bank.
    /// </summary>
    [Serializable]
    public abstract class DepositAccount : Pondus, IAccount<Identifier>
    {
        #region ctor

        protected DepositAccount(DateTime dateOpenned) : base(dateOpenned)
        {
        }

        #endregion

        #region properties

        public bool IsJointAcct { get; set; }
        public virtual Identifier Id { get; set; }

        #endregion

        #region methods

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, 0.0F);
        }

        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var d = Balance.GetCurrent(dt, 0.0F);
            return d < Pecuniam.Zero ? d.Abs : Pecuniam.Zero;
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
                AddNegativeValue(dt, amount, new Mereo(note));
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
                AddPositiveValue(dt, amount, new Mereo(note));
        }

        /// <summary>
        /// Moves <see cref="amt"/> out of <see cref="fromAccount"/> to <see cref="toAccount"/>.
        /// When the amount exceeds what is available it will be divided in half continously until
        /// it fits or goes down to one penny.
        /// </summary>
        /// <param name="fromAccount"></param>
        /// <param name="toAccount"></param>
        /// <param name="amt"></param>
        /// <param name="dt"></param>
        public static void TransferFundsInBankAccounts(DepositAccount fromAccount, DepositAccount toAccount,
            Pecuniam amt, DateTime dt)
        {
            if (fromAccount == null || toAccount == null || amt == null || amt == Pecuniam.Zero)
                return;
            if (fromAccount.GetStatus(dt) != SpStatus.Current && toAccount.GetStatus(dt) != SpStatus.Current)
                return;

            if (fromAccount.Inception < dt
                || toAccount.Inception < dt)
                return;
            amt = amt.Abs;

            while (fromAccount.Value < amt)
            {
                amt = amt / 2.ToPecuniam();
                if (amt.Amount < 0.01M)
                    break;
            }
            fromAccount.Withdraw(dt, amt);
            toAccount.Deposit(dt.AddMilliseconds(100), amt);
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
            var dtd = dt.GetValueOrDefault(DateTime.Now);
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
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.RandomRChars(true));
            return new SavingsAccount(accountId, dtd);
        }

        #endregion
    }
}