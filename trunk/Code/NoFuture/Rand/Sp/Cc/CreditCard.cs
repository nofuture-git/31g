using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Sp.Cc
{
    /// <summary>
    /// Creates new instance with properties randomly assigned, 
    /// All date-time fields are converted to UTC
    /// </summary>
    [Serializable]
    public abstract class CreditCard : ICreditCard
    {
        #region ctor
        protected CreditCard():this(null, null, null){ }

        protected CreditCard(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
        {
            CardHolderSince = openedDate.GetValueOrDefault(DateTime.UtcNow);

            if (expiryDate == null)
            {
                ExpDate = Etx.RandomDate(Etx.RandomInteger(4, 6), null);
                ExpDate = new DateTime(ExpDate.Year, ExpDate.Month, Etx.RandomCoinToss() ? 1 : 15);
            }
            else
            {
                ExpDate = expiryDate.Value;
            }
            if (cardholder != null)
            {
                var fname = (cardholder.GetName(KindsOfNames.First) ?? String.Empty).ToUpper();
                var lname = (cardholder.GetName(KindsOfNames.Surname) ?? String.Empty).ToUpper();
                CardHolderName = string.IsNullOrWhiteSpace(fname) && string.IsNullOrWhiteSpace(lname)
                    ? (cardholder.GetName(KindsOfNames.Legal) ?? String.Empty).ToUpper()
                    : String.Join(" ", fname, lname);
            }
            
            Cvv = $"{Etx.RandomInteger(7, 999),3:D3}";
            Number = GetRandomCardNumber();
        }

        protected CreditCard(string cardNumber, IVoca cardholder, DateTime? openedDate, DateTime? expiryDate) : this(
            cardholder, openedDate, expiryDate)
        {
            var ccNum = GetRandomCardNumber();
            if (string.IsNullOrWhiteSpace(cardNumber))
                return;
            if (!ccNum.Validate(cardNumber))
                throw new ArgumentException($"The card number {cardNumber} is not valid.");
            ccNum.Value = cardNumber;
        }

        protected CreditCard(string cardNumber, string cvv, IVoca cardholder, DateTime? openedDate,
            DateTime? expiryDate) : this(cardNumber, cardholder, openedDate, expiryDate)
        {
            if (cvv == null)
                return;
            if (!Regex.IsMatch(cvv, "[0-9]{3}"))
                throw new ArgumentException($"The CVV value {cvv} is not valid.");
            Cvv = cvv;
        }
        #endregion

        #region properties
        public CreditCardNumber Number { get; }
        public DateTime ExpDate { get; }
        public string CardHolderName { get; }
        public string Cvv { get; }
        public DateTime CardHolderSince { get; }

        protected internal abstract int CardNumLen { get; }
        protected internal abstract Rchar[] CardNumPrefix { get; }
        public abstract string CcName { get; }
        #endregion

        #region methods
        protected internal CreditCardNumber GetRandomCardNumber()
        {
            var prefixValLen = CardNumPrefix.Length;
            var prefixRChars = CardNumPrefix.ToList();
            prefixRChars.AddRange(Etx.RandomRChars(true, CardNumLen - 1 - prefixValLen, prefixValLen));
            return new CreditCardNumber(prefixRChars.ToArray(), CcName);
        }

        /// <summary>
        /// Returns the credit card in a format
        /// like what is on a receipt.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(" ", Number.ValueLastFour(), CardHolderName);
        }

        /// <summary>
        /// Returs a new, randomly gen&apos;ed, concrete instance of <see cref="ICreditCard"/>
        /// </summary>
        /// <param name="cardholder"></param>
        /// <param name="opennedDate"></param>
        /// <returns></returns>
        [RandomFactory]
        public static ICreditCard RandomCreditCard(IVoca cardholder, DateTime? opennedDate = null)
        {
            var fk = Etx.RandomInteger(0, 3);
            var dt = opennedDate ?? Etx.RandomDate(-3, null);

            switch (fk)
            {
                case 0:
                    return new MasterCardCc(cardholder, dt, dt.AddYears(3));
                case 2:
                    return new AmexCc(cardholder, dt, dt.AddYears(3));
                case 3:
                    return new DiscoverCc(cardholder, dt, dt.AddYears(3));
                default:
                    return new VisaCc(cardholder, dt, dt.AddYears(3));
            }
        }

        /// <summary>
        /// Returs a new, randomly gen&apos;ed, concrete instance of <see cref="ICreditCard"/>
        /// </summary>
        /// <param name="cardholder"></param>
        /// <param name="opennedDate"></param>
        /// <returns></returns>
        [RandomFactory]
        public static ICreditCard RandomCreditCard(string cardholder = null, DateTime? opennedDate = null)
        {
            return RandomCreditCard(new VocaBase(cardholder ?? ""), opennedDate);
        }

        /// <summary>
        /// Gets the kind of <see cref="ICreditCard"/> whose pattern matches <see cref="cardNumber"/>
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="cardHolder"></param>
        /// <param name="cvv"></param>
        /// <param name="opennedDate"></param>
        /// <param name="expiryDate"></param>
        /// <returns></returns>
        /// <remarks>
        /// Throws <see cref="ArgumentException"/> if <see cref="cardNumber"/> does not match any defined pattern
        /// </remarks>
        [RandomFactory]
        public static ICreditCard RandomCreditCard(string cardNumber, string cardHolder, string cvv = null,
            DateTime? opennedDate = null, DateTime? expiryDate = null)
        {
            if (MasterCardCc.IsValidNumber(cardNumber))
                return new MasterCardCc(cardNumber, cvv, new VocaBase(cardNumber ?? ""), opennedDate, expiryDate );
            if(VisaCc.IsValidNumber(cardNumber))
                return new VisaCc(cardNumber, cvv, new VocaBase(cardNumber ?? ""), opennedDate, expiryDate);
            if(AmexCc.IsValidNumber(cardNumber))
                return new AmexCc(cardNumber, cvv, new VocaBase(cardNumber ?? ""), opennedDate, expiryDate);
            if(DiscoverCc.IsValidNumber(cardNumber))
                return new DiscoverCc(cardNumber, cvv, new VocaBase(cardNumber ?? ""), opennedDate, expiryDate);

            throw new ArgumentException($"The card number {cardNumber} does not match any of the defined credit cards.");

        }
        #endregion
    }
}
