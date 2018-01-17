using System;
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
        /// Helper method to copy the debit transactions within any of <see cref="receivables"/>
        /// whose <see cref="ITransaction.AtTime"/> is on the <see cref="dt"/>.
        /// </summary>
        /// <param name="dt">Defaults to today, ranges between 00:00:00.000 and 23:59:59.999 hours</param>
        /// <param name="receivables"></param>
        public void AddDebitTransactionsByDate(DateTime? dt, IList<IReceivable> receivables)
        {
            if (receivables == null || receivables.Count <= 0)
                return;

            var stDt = dt.GetValueOrDefault(DateTime.Now).Date;
            var endDt = dt.GetValueOrDefault(DateTime.Now).Date.AddDays(1).AddMilliseconds(-1);

            if (Terminus != null && stDt > Terminus.Value.Date)
                return;

            var trans = receivables.SelectMany(x => x.Balance.GetTransactionsBetween(stDt, endDt, true));

            foreach (var t in trans)
            {
                Pop(t.AtTime.AddMilliseconds(1), t.Cash, t.Description, Pecuniam.Zero);
            }
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
            fromAccount.Pop(dt, amt,null, Pecuniam.Zero);
            toAccount.Push(dt.AddMilliseconds(100), amt, null, Pecuniam.Zero);
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
        public static CheckingAccount RandomCheckingAccount(IVoca personName, DateTime? dt = null, string debitPin = null)
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
        public static SavingsAccount RandomSavingAccount(IVoca personName, DateTime? dt = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.RandomRChars(true));
            return new SavingsAccount(accountId, dtd);
        }

        #endregion
    }
}