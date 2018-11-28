using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Cc;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class CheckingAccount : DepositAccount
    {
        #region fields
        private readonly byte[] _pinKey;
        private readonly byte[] _pinHash;
        #endregion

        #region ctor

        public CheckingAccount(string acctId, DateTime dateOpenned, Tuple<ICreditCard, string> debitCard = null) : this(
            new AccountId(acctId), dateOpenned, debitCard)
        {
        }

        /// <summary>
        /// Creates new Checking Deposit account instance
        /// </summary>
        /// <param name="acctId"></param>
        /// <param name="dateOpenned"></param>
        /// <param name="debitCard">
        /// Item2 is the PIN number and must be 4 numerical chars. 
        /// Its value is hashed and not stored within the instance.
        /// </param>
        public CheckingAccount(Identifier acctId, DateTime dateOpenned, Tuple<ICreditCard, string> debitCard = null) :
            base(acctId, dateOpenned)
        {
            if (debitCard?.Item1 == null || !IsPossiablePin(debitCard.Item2))
                return;
            DebitCard = debitCard.Item1;
            _pinKey = Encoding.UTF8.GetBytes(Path.GetRandomFileName());
            _pinHash = ComputePinHash(debitCard.Item2);
        }

        #endregion

        #region properties
        public override Pecuniam Value => Balance.GetCurrent(DateTime.UtcNow, 0F);
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

        public static bool IsPossiablePin(string somestring)
        {
            return !string.IsNullOrWhiteSpace(somestring) && Regex.IsMatch(somestring, "[0-9]{4}");
        }

        #endregion
    }
}