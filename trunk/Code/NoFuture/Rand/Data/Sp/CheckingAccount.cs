using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp
{
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
}