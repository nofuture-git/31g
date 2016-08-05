using System;
using System.Collections.Generic;
using System.Text;
using NoFuture.Rand.Domus;
using NoFuture.Util;

namespace NoFuture.Rand.Data.Sp
{
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

    [Serializable]
    public abstract class CreditCard : FixedRateLoan
    {
        #region constants
        public const float DF_MIN_PMT_RATE = 0.0125F;
        #endregion

        #region ctor
        protected CreditCard(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(openedDate, minPaymentRate <= 0 ? DF_MIN_PMT_RATE : minPaymentRate, amt)
        {
            ExpDate = Etx.Date(Etx.IntNumber(2, 5), null);
            ExpDate = new DateTime(ExpDate.Year, ExpDate.Month, Etx.CoinToss ? 1 : 15);
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

        public override string ToString()
        {
            var bldr = new StringBuilder();
            var val = Number?.Value;
            if(string.IsNullOrWhiteSpace(val))
                return base.ToString();

            for (var i = 0; i < val.Length - 4; i++)
            {
                bldr.Append("X");
            }
            var lastFour = val.Substring(val.Length - 4, 4);
            bldr.Append(lastFour);

            return string.Join(" ", bldr.ToString(), CcName);
        }

        #endregion
    }

    [Serializable]
    public class MasterCardCc : CreditCard
    {
        public MasterCardCc(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(cardholder, openedDate, minPaymentRate, amt)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => Etx.IntNumber(51, 55);
        protected override string CcName => "MC";
    }

    [Serializable]
    public class VisaCc : CreditCard
    {
        public VisaCc(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(cardholder, openedDate, minPaymentRate, amt)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => 4;
        protected override string CcName => "VISA";
    }

    [Serializable]
    public class AmexCc : CreditCard
    {
        public AmexCc(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(cardholder, openedDate, minPaymentRate, amt)
        {
        }

        protected override int CardNumLen => 15;
        protected override int CardNumPrefix => Etx.CoinToss ? 34 : 37;
        protected override string CcName => "AMEX";
    }

    [Serializable]
    public class DiscoverCc : CreditCard
    {
        public DiscoverCc(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(cardholder, openedDate, minPaymentRate, amt)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => 6011;
        protected override string CcName => "DC";
    }
}
