using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public enum AccountStatus
    {
        Closed,
        Current,
        Late,
        NoHistory
    }
    [Serializable]
    public class AccountId : RIdentifier
    {
        public AccountId(Rchar[] format)
        {
            this.format = format;
        }

        public override string Abbrev => "Acct";
    }

    [Serializable]
    public abstract class DepositAccount : IAsset, ITransactionable
    {
        #region ctor

        protected DepositAccount(DateTime dateOpenned)
        {
            Balance = new Balance();
            OpenDate = dateOpenned.ToUniversalTime();
        }

        #endregion 

        #region properties
        public bool IsJointAcct { get; set; }
        public AccountId AccountNumber { get; set; }
        public IBalance Balance { get; }
        public FinancialFirm Bank { get; set; }
        public abstract Pecuniam Value { get; }
        public DateTime OpenDate { get; }
        #endregion

        #region methods
        public override string ToString()
        {
            return string.Join(" ", GetType().Name, Bank, AccountNumber);
        }

        public virtual void PutCashIn(DateTime dt, Pecuniam val, string note = null)
        {
            if (val == Pecuniam.Zero)
                return;
            Balance.AddTransaction(dt, val.Abs, note);
        }

        public virtual bool TakeCashOut(DateTime dt, Pecuniam val, string note = null)
        {
            if (val == Pecuniam.Zero)
                return true;

            if (val > Balance.GetCurrent(dt, 0F))
                return false;
            Balance.AddTransaction(dt, val.Neg, note);
            return true;
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
        public CheckingAccount(DateTime dateOpenned, Tuple<ICreditCard, string> debitCard = null) : base(dateOpenned)
        {
            if (debitCard?.Item1 == null || string.IsNullOrWhiteSpace(debitCard.Item2) || !Regex.IsMatch(debitCard.Item2, "[0-9]{4}") )
                return;
            DebitCard = debitCard.Item1;
            _pinKey = Encoding.UTF8.GetBytes(Path.GetRandomFileName());
            _pinHash = ComputePinHash(debitCard.Item2);
        }
        #endregion

        #region properties
        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, 0F);
        public virtual ICreditCard DebitCard { get; }
        #endregion

        #region methods
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

        public static CheckingAccount GetRandomCheckingAcct(IPerson p, DateTime? dt = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            var bank = Com.Bank.GetRandomBank(p?.Address?.HomeCityArea);
            return new CheckingAccount(dtd,
                new Tuple<ICreditCard, string>(CreditCard.GetRandomCreditCard(p), $"{Etx.IntNumber(7, 9999),4:D4}"))
            {
                AccountNumber = accountId,
                Bank = bank
            };
        }
        #endregion
    }

    [Serializable]
    public class SavingsAccount : DepositAccount
    {
        public SavingsAccount(DateTime dateOpenned) : base(dateOpenned)
        {
        }

        public float InterestRate { get; set; }
        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, InterestRate);

        public static SavingsAccount GetRandomSavingAcct(IPerson p, DateTime? dt = null)
        {
            var dtd = dt.GetValueOrDefault(DateTime.Now);
            var accountId = new AccountId(Etx.GetRandomRChars(true));
            var bank = Com.Bank.GetRandomBank(p?.Address?.HomeCityArea);
            return new SavingsAccount(dtd) {AccountNumber = accountId, Bank = bank};
        }
    }

    /// <summary>
    /// Represents a personal credit card in the form of 
    /// both its properties (e.g. owner, expiry, etc) and
    /// the history of transactions and payments.
    /// </summary>
    [Serializable]
    public class CreditCardAccount : FixedRateLoan
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
        public Pecuniam Max => base.TradeLine.CreditLimit;
        public ICreditCard Cc { get; }
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
            return GetCurrentBalance(dt) >= Max;
        }

        /// <summary>
        /// Applies a purchase transation to this credit card.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="val"></param>
        /// <param name="note"></param>
        /// <returns>
        /// True when the card is not expired and
        /// the purchase amount <see cref="val"/>
        /// will not cause the total balance to exceed <see cref="Max"/>.
        /// </returns>
        public override bool TakeCashOut(DateTime dt, Pecuniam val, string note = null)
        {
            if (dt > Cc.ExpDate)
                return false;
            var cBal = GetCurrentBalance(dt);
            if (cBal >= Max || cBal + val >= Max)
                return false;
            return base.TakeCashOut(dt, val, note);
        }

        /// <summary>
        /// Returns the credit card in a format
        /// like what is on a receipt.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var bldr = new StringBuilder();
            var val = Cc.Number?.Value;
            if (string.IsNullOrWhiteSpace(val))
                return base.ToString();

            for (var i = 0; i < val.Length - 4; i++)
            {
                bldr.Append("X");
            }
            var lastFour = val.Substring(val.Length - 4, 4);
            bldr.Append(lastFour);

            return string.Join(" ", bldr.ToString(), Cc.CardHolderName);
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
            var cc = CreditCard.GetRandomCreditCard(p);
            var max = ccScore == null ? new Pecuniam(1000) : ccScore.GetRandomMax(cc.CardHolderSince);
            var randRate = ccScore?.GetRandomInterestRate(cc.CardHolderSince, baseInterestRate) * 0.01 ?? baseInterestRate;
            var ccAcct = new CreditCardAccount(cc, minPmtPercent, max) {Rate = (float) randRate};
            return ccAcct;
        }
        #endregion
    }
}
