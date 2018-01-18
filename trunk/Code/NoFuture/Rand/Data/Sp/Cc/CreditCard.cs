using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Data.Sp.Cc
{
    /// <summary>
    /// Creates new instance with properties randomly assigned, 
    /// All date-time fields are converted to UTC
    /// </summary>
    [Serializable]
    public abstract class CreditCard : ICreditCard
    {
        #region ctor
        protected CreditCard(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
        {
            CardHolderSince = openedDate.GetValueOrDefault(DateTime.Now);

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
        #endregion

        #region properties
        public CreditCardNumber Number { get; }
        public DateTime ExpDate { get; }
        public string CardHolderName { get; }
        public string Cvv { get; }
        public DateTime CardHolderSince { get; }

        protected internal abstract int CardNumLen { get; }
        protected internal abstract int CardNumPrefix { get; }
        public abstract string CcName { get; }
        #endregion

        #region methods
        protected internal CreditCardNumber GetRandomCardNumber()
        {
            var prefixVal = CardNumPrefix;
            var prefixValLen = prefixVal.ToString().Length;
            var prefixRChars = new List<Rchar>();
            for (var i = 0; i < prefixValLen; i++)
            {
                prefixRChars.Add(new RcharLimited(i, prefixVal.ToString().ToCharArray()[i]));
            }
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
            return String.Join(" ", Number.ValueLastFour(), CardHolderName);
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
            var voca = new VocaBase();
            voca.UpsertName(KindsOfNames.Legal, cardholder ?? "");
            return RandomCreditCard(voca, opennedDate);
        }

        #endregion
    }
}
