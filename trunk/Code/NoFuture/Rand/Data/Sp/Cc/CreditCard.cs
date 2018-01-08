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
                ExpDate = Etx.Date(Etx.IntNumber(4, 6), null);
                ExpDate = new DateTime(ExpDate.Year, ExpDate.Month, Etx.CoinToss ? 1 : 15);
            }
            else
            {
                ExpDate = expiryDate.Value;
            }
            if (cardholder != null)
            {
                var fname = (cardholder.GetName(KindsOfNames.First) ?? string.Empty).ToUpper();
                var lname = (cardholder.GetName(KindsOfNames.Surname) ?? string.Empty).ToUpper();
                CardHolderName = string.Join(" ", fname, lname);
            }
            
            Cvv = $"{Etx.IntNumber(7, 999),3:D3}";
            Number = GetRandomCardNumber();
        }
        #endregion

        #region properties
        public CreditCardNumber Number { get; }
        public DateTime ExpDate { get; }
        public string CardHolderName { get; }
        public string Cvv { get; }
        public DateTime CardHolderSince { get; }

        protected abstract int CardNumLen { get; }
        protected abstract int CardNumPrefix { get; }
        protected abstract string CcName { get; }
        #endregion

        #region methods
        protected CreditCardNumber GetRandomCardNumber()
        {
            var prefixVal = CardNumPrefix;
            var prefixValLen = prefixVal.ToString().Length;
            var prefixRChars = new List<Rchar>();
            for (var i = 0; i < prefixValLen; i++)
            {
                prefixRChars.Add(new RcharLimited(i, prefixVal.ToString().ToCharArray()[i]));
            }
            prefixRChars.AddRange(Etx.GetRandomRChars(true, CardNumLen - 1 - prefixValLen, prefixValLen));
            return new CreditCardNumber(prefixRChars.ToArray());
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

        #endregion
    }
}
