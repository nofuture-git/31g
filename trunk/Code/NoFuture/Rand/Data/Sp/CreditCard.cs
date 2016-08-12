﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Domus;
using NoFuture.Util;

namespace NoFuture.Rand.Data.Sp
{


    /// <summary>
    /// Represents a credit card number with algo for check digit
    /// </summary>
    /// <remarks>
    /// Given the format as an ordered-array of <see cref="Rchar"/>
    /// this type can both create random values and validate them.
    /// </remarks>
    [Serializable]
    public class CreditCardNumber : RIdentifierWithChkDigit
    {
        public CreditCardNumber(Rchar[] format)
        {
            CheckDigitFunc = Etc.CalcLuhnCheckDigit;
            this.format = format;
        }
        public override string Abbrev => "CC Num";
    }

    /// <summary>
    /// Creates new instance with properties randomly assigned, 
    /// All date-time fields are converted to UTC
    /// </summary>
    [Serializable]
    public abstract class CreditCard : ICreditCard
    {
        #region ctor
        protected CreditCard(IPerson cardholder, DateTime? openedDate, DateTime? expiryDate)
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

            CardHolderName = string.Join(" ", cardholder.FirstName.ToUpper(), cardholder.LastName.ToUpper());
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
                prefixRChars.Add(new LimitedRchar(i, prefixVal.ToString().ToCharArray()[i]));
            }
            prefixRChars.AddRange(Etx.GetRandomRChars(true, CardNumLen - 1 - prefixValLen, prefixValLen));
            return new CreditCardNumber(prefixRChars.ToArray());
        }

        /// <summary>
        /// Returs a new, randomly gen'ed, concrete instance of <see cref="ICreditCard"/>
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static ICreditCard GetRandomCreditCard(IPerson p)
        {
            var fk = Etx.IntNumber(0, 3);
            var dt = Etx.Date(-2, null);

            switch (fk)
            {
                case 0:
                    return new MasterCardCc(p, dt, dt.AddYears(4));
                case 2:
                    return new AmexCc(p, dt, dt.AddYears(4));
                case 3:
                    return new DiscoverCc(p, dt, dt.AddYears(4));
                default:
                    return new VisaCc(p, dt, dt.AddYears(4));
            }
        }
        #endregion
    }

    [Serializable]
    public class MasterCardCc : CreditCard
    {
        public MasterCardCc(IPerson cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => Etx.IntNumber(51, 55);
        protected override string CcName => "MC";
    }

    [Serializable]
    public class VisaCc : CreditCard
    {
        public VisaCc(IPerson cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => 4;
        protected override string CcName => "VISA";
    }

    [Serializable]
    public class AmexCc : CreditCard
    {
        public AmexCc(IPerson cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        protected override int CardNumLen => 15;
        protected override int CardNumPrefix => Etx.CoinToss ? 34 : 37;
        protected override string CcName => "AMEX";
    }

    [Serializable]
    public class DiscoverCc : CreditCard
    {
        public DiscoverCc(IPerson cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => 6011;
        protected override string CcName => "DC";
    }
}
