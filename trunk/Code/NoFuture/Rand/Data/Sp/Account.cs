using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class AccountId : RIdentifier
    {
        public AccountId(Rchar[] format)
        {
            this.format = format;
        }

        public override string Abbrev => "Acct";
    }

    /// <summary>
    /// Base type for a depository account held at a commercial bank.
    /// </summary>
    [Serializable]
    public abstract class DepositAccount : IAccount<Identifier>, ITransactionable
    {
        #region ctor
        protected DepositAccount(DateTime dateOpenned)
        {
            Balance = new Balance();
            Inception = dateOpenned;
        }
        #endregion 

        #region properties
        public bool IsJointAcct { get; set; }
        public virtual Identifier Id { get; set; }
        public IBalance Balance { get; }
        public FinancialFirm Bank { get; set; }
        public abstract Pecuniam CurrentValue { get; }
        public virtual DateTime Inception { get; }
        public virtual DateTime? Terminus { get; set; }
        #endregion

        #region methods
        public override string ToString()
        {
            return string.Join(" ", GetType().Name, Bank, Id);
        }

        public virtual Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, 0.0F);
        }

        public SpStatus GetStatus(DateTime? dt)
        {
            if(Terminus != null && Terminus < dt)
                return SpStatus.Closed;
            var ddt = dt ?? DateTime.Now;

            var balAtDt = Balance.GetCurrent(ddt, 0F);
            return balAtDt < Pecuniam.Zero ? SpStatus.Short : SpStatus.Current;
        }

        public virtual void Push(DateTime dt, Pecuniam val, Pecuniam fee = null, string note = null)
        {
            if (val == Pecuniam.Zero)
                return;
            Balance.AddTransaction(dt, val.Abs, fee, note);
        }

        public virtual bool Pop(DateTime dt, Pecuniam val, Pecuniam fee = null, string note = null)
        {
            if (val == Pecuniam.Zero)
                return true;
            if (GetStatus(dt) != SpStatus.Current)
                return false;
            if (val > Balance.GetCurrent(dt, 0F))
                return false;
            Balance.AddTransaction(dt, val.Neg, fee, note);
            return true;
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

            var trans = receivables.SelectMany(x => x.TradeLine.Balance.GetTransactionsBetween(stDt, endDt, true));

            foreach (var t in trans)
            {
                Pop(t.AtTime.AddMilliseconds(1), (Pecuniam)t.Cash, Pecuniam.Zero, t.Description);
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
        public static void TransferFundsInBankAccounts(DepositAccount fromAccount, DepositAccount toAccount, Pecuniam amt, DateTime dt)
        {
            if (fromAccount == null || toAccount == null || amt == null || amt == Pecuniam.Zero)
                return;
            if (fromAccount.GetStatus(dt) != SpStatus.Current && toAccount.GetStatus(dt) != SpStatus.Current)
                return;

            if (fromAccount.Inception < dt || toAccount.Inception < dt)
                return;
            amt = amt.Abs;

            while (fromAccount.CurrentValue < amt)
            {
                amt = amt / 2.ToPecuniam();
                if (amt.Amount < 0.01M)
                    break;
            }
            fromAccount.Pop(dt, amt, Pecuniam.Zero, Opes.GetPaymentNote(fromAccount.Id));
            toAccount.Push(dt.AddMilliseconds(100), amt, Pecuniam.Zero, Opes.GetPaymentNote(toAccount.Id));
        }

        #endregion
    }

    [Serializable]
    public class CheckingAccount : DepositAccount
    {
        #region fields
        private readonly byte[] _pinKey;
        private readonly byte[] _pinHash;
        #endregion

        #region ctor
        /// <summary>
        /// Creates new Checking Deposit account instance
        /// </summary>
        /// <param name="acctId"></param>
        /// <param name="dateOpenned"></param>
        /// <param name="debitCard">
        /// Item2 is the PIN number and must be 4 numerical chars. 
        /// Its value is hashed and not stored within the instance.
        /// </param>
        public CheckingAccount(RIdentifier acctId, DateTime dateOpenned, Tuple<ICreditCard, string> debitCard = null) : base(dateOpenned)
        {
            Id = acctId;
            if (debitCard?.Item1 == null || !IsPossiablePin(debitCard.Item2) )
                return;
            DebitCard = debitCard.Item1;
            _pinKey = Encoding.UTF8.GetBytes(Path.GetRandomFileName());
            _pinHash = ComputePinHash(debitCard.Item2);
        }
        #endregion

        #region properties
        public override Pecuniam CurrentValue => Balance.GetCurrent(DateTime.Now, 0F);
        public virtual ICreditCard DebitCard { get; }
        #endregion

        #region methods
        /// <summary>
        /// Returns true if <see cref="tryPin"/>
        /// equals the PIN assigned at ctor-time.
        /// </summary>
        /// <param name="tryPin"></param>
        /// <returns></returns>
        public bool IsPin(string tryPin)
        {
            if (_pinHash == null || _pinKey == null)
                return false;
            var tryPinBuffer = ComputePinHash(tryPin);
            return tryPinBuffer.SequenceEqual(_pinHash);
        }

        protected byte[] ComputePinHash(string pinNum)
        {
            var hmac = System.Security.Cryptography.HMAC.Create();
            hmac.Key = _pinKey;
            var pinBuffer = new byte[4];
            Array.Copy(Encoding.UTF8.GetBytes(pinNum), pinBuffer, 4);
            return hmac.ComputeHash(pinBuffer);
        }

        internal static bool IsPossiablePin(string somestring)
        {
            return !string.IsNullOrWhiteSpace(somestring) && Regex.IsMatch(somestring, "[0-9]{4}");
        }

        /// <summary>
        /// Creates a new random Checking Account
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dt">Date account was openned, default to now.</param>
        /// <param name="debitPin">
        /// Optional, when present and random instance of <see cref="DebitCard"/> is created with 
        /// this as its PIN.
        /// </param>
        /// <returns></returns>
        public static CheckingAccount GetRandomCheckingAcct(IPerson p, DateTime? dt = null, string debitPin = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            var bank = Com.Bank.GetRandomBank(p?.Address?.HomeCityArea);
            return IsPossiablePin(debitPin)
                ? new CheckingAccount(accountId, dtd,
                    new Tuple<ICreditCard, string>(CreditCard.GetRandomCreditCard(p), debitPin))
                {
                    Bank = bank
                }
                : new CheckingAccount(accountId, dtd)
                {
                    Bank = bank
                };
        }
        #endregion
    }

    [Serializable]
    public class SavingsAccount : DepositAccount
    {
        #region ctor
        public SavingsAccount(RIdentifier acctId, DateTime dateOpenned) : base(dateOpenned)
        {
            Id = acctId;
        }
        #endregion

        #region properties
        public float InterestRate { get; set; }
        public override Pecuniam CurrentValue => Balance.GetCurrent(DateTime.Now, InterestRate);
        #endregion

        #region methods

        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, InterestRate);
        }

        public static SavingsAccount GetRandomSavingAcct(IPerson p, DateTime? dt = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            var bank = Com.Bank.GetRandomBank(p?.Address?.HomeCityArea);
            return new SavingsAccount(accountId,dtd) {Bank = bank};
        }
        #endregion
    }

    /// <summary>
    /// Represents a personal credit card in the form of 
    /// both its properties (e.g. owner, expiry, etc) and
    /// the history of transactions and payments.
    /// </summary>
    [Serializable]
    public class CreditCardAccount : FixedRateLoan, IAccount<Identifier>
    {
        #region constants
        public const float DF_MIN_PMT_RATE = 0.0125F;
        #endregion

        #region ctor
        public CreditCardAccount(ICreditCard cc, float minPaymentRate, Pecuniam ccMax = null)
            : base(cc.CardHolderSince, minPaymentRate <= 0 ? DF_MIN_PMT_RATE : minPaymentRate, null)
        {
            Cc = cc;
            base.TradeLine.CreditLimit = ccMax ?? new Pecuniam(1000);
            base.TradeLine.FormOfCredit = FormOfCredit.Revolving;
            base.TradeLine.DueFrequency = new TimeSpan(30, 0, 0, 0);
        }
        #endregion

        #region properties
        public Pecuniam Max => TradeLine.CreditLimit;
        public ICreditCard Cc { get; }
        public Identifier Id => Cc.Number;
        public DateTime Inception => TradeLine.OpennedDate;

        public DateTime? Terminus
        {
            get { return TradeLine.Closure?.ClosedDate; }
            set
            {
                if (value.HasValue)
                    TradeLine.Closure = new TradelineClosure {ClosedDate = value.Value};
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Public API method to allow the <see cref="Max"/> to 
        /// be increased and only increased.
        /// </summary>
        /// <param name="val"></param>
        public void IncreaseMaxTo(Pecuniam val)
        {
            if (Max != null && Max > val)
                return;
            base.TradeLine.CreditLimit = val;
        }

        /// <summary>
        /// Asserts that the current balance equals-or-exceeds
        /// this instances <see cref="Max"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool IsMaxedOut(DateTime dt)
        {
            return GetValueAt(dt) >= Max;
        }

        /// <summary>
        /// Applies a purchase transation to this credit card.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="val"></param>
        /// <param name="fee"></param>
        /// <param name="note"></param>
        /// <returns>
        /// True when the card is not expired and
        /// the purchase amount <see cref="val"/>
        /// will not cause the total balance to exceed <see cref="Max"/>.
        /// </returns>
        public override bool Pop(DateTime dt, Pecuniam val, Pecuniam fee = null, string note = null)
        {
            if (dt > Cc.ExpDate)
                return false;
            var cBal = GetValueAt(dt);
            if (cBal >= Max || cBal + val >= Max)
                return false;
            return base.Pop(dt, val, fee, note);
        }

        /// <summary>
        /// Returns the credit card in a format
        /// like what is on a receipt.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Cc?.ToString() ?? base.ToString();
        }

        /// <summary>
        /// Randomly gen's one of the concrete types of <see cref="CreditCardAccount"/>.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ccScore">
        /// Optional, if given then will generate an interest-rate and cc-max 
        /// in accordance with the score.
        /// </param>
        /// <param name="baseInterestRate">
        /// This is the lowest possiable interest rate for the random generators
        /// </param>
        /// <param name="minPmtPercent">
        /// The value used to calc a minimum monthly payment
        /// </param>
        /// <returns></returns>
        public static CreditCardAccount GetRandomCcAcct(IPerson p, CreditScore ccScore,
            float baseInterestRate = 10.1F + Gov.Fed.RiskFreeInterestRate.DF_VALUE,
            float minPmtPercent = DF_MIN_PMT_RATE)
        {
            if(ccScore == null && p is NorthAmerican)
                ccScore = new PersonalCreditScore((NorthAmerican) p);

            var cc = CreditCard.GetRandomCreditCard(p);
            var max = ccScore == null ? new Pecuniam(1000) : ccScore.GetRandomMax(cc.CardHolderSince);
            var randRate = ccScore?.GetRandomInterestRate(cc.CardHolderSince, baseInterestRate) * 0.01 ?? baseInterestRate;
            var ccAcct = new CreditCardAccount(cc, minPmtPercent, max) {Rate = (float) randRate};
            return ccAcct;
        }
        #endregion
    }
}
