using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoFuture.Rand.Com;
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
    /// Represents a personal credit card in the form of 
    /// both its properties (e.g. owner, expiry, etc) and
    /// the history of transactions and payments.
    /// </summary>
    [Serializable]
    public abstract class CreditCard : FixedRateLoan
    {
        #region constants
        public const float DF_MIN_PMT_RATE = 0.0125F;
        #endregion

        #region ctor
        protected CreditCard(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam ccMax = null)
            : base(openedDate, minPaymentRate <= 0 ? DF_MIN_PMT_RATE : minPaymentRate, null)
        {
            ExpDate = Etx.Date(Etx.IntNumber(4, 6), null);
            ExpDate = new DateTime(ExpDate.Year, ExpDate.Month, Etx.CoinToss ? 1 : 15);
            
            CardHolderName = string.Join(" ", cardholder.FirstName.ToUpper(), cardholder.LastName.ToUpper());
            Cvv = $"{Etx.IntNumber(7, 999),3:D3}";
            Number = GetRandomCardNumber();
            base.TradeLine.CreditLimit = ccMax ?? new Pecuniam(1000);
            base.TradeLine.FormOfCredit = FormOfCredit.Revolving;
            base.TradeLine.DueFrequency = new TimeSpan(30,0,0,0);

        }
        #endregion

        #region properties
        public CreditCardNumber Number { get; }
        public DateTime ExpDate { get; }
        public string CardHolderName { get; }
        public string Cvv { get; }
        public DateTime CardHolderSince => TradeLine.OpennedDate;
        public Pecuniam Max => base.TradeLine.CreditLimit;

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
        /// <returns>
        /// True when the card is not expired and
        /// the purchase amount <see cref="val"/>
        /// will not cause the total balance to exceed <see cref="Max"/>.
        /// </returns>
        public override bool MakeAPurchase(DateTime dt, Pecuniam val)
        {
            if (dt > ExpDate)
                return false;
            var cBal = GetCurrentBalance(dt);
            if (cBal >= Max || cBal + val >= Max)
                return false;
            return base.MakeAPurchase(dt, val);
        }

        /// <summary>
        /// Returns the credit card in a format
        /// like what is on a receipt.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Randomly gen's one of the concrete types of <see cref="CreditCard"/>.
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
        public static CreditCard GetRandomCc(IPerson p, CreditScore ccScore,
            float baseInterestRate = 10.1F + Gov.Fed.RiskFreeInterestRate.DF_VALUE,
            float minPmtPercent = DF_MIN_PMT_RATE)
        {
            CreditCard cc;
            var fk = Etx.IntNumber(0, 3);
            var dt = Etx.Date(-2, null);
            var max = ccScore == null ? new Pecuniam(1000) : ccScore.GetRandomMax(dt);
            var randRate = ccScore?.GetRandomInterestRate(dt, baseInterestRate)*0.01 ?? baseInterestRate;
            switch (fk)
            {
                case 0:
                    cc = new MasterCardCc(p, dt, minPmtPercent, max);
                    break;
                case 2:
                    cc = new AmexCc(p, dt, minPmtPercent, max);
                    break;
                case 3:
                    cc = new DiscoverCc(p, dt, minPmtPercent, max);
                    break;
                default:
                    cc = new VisaCc(p, dt, minPmtPercent, max);
                    break;
            }
            cc.Rate = (float) randRate;
            return cc;
        }

        #endregion
    }

    [Serializable]
    public class MasterCardCc : CreditCard
    {
        public MasterCardCc(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam ccMax = null)
            : base(cardholder, openedDate, minPaymentRate, ccMax)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => Etx.IntNumber(51, 55);
        protected override string CcName => "MC";
    }

    [Serializable]
    public class VisaCc : CreditCard
    {
        public VisaCc(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam ccMax = null)
            : base(cardholder, openedDate, minPaymentRate, ccMax)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => 4;
        protected override string CcName => "VISA";
    }

    [Serializable]
    public class AmexCc : CreditCard
    {
        public AmexCc(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam ccMax = null)
            : base(cardholder, openedDate, minPaymentRate, ccMax)
        {
        }

        protected override int CardNumLen => 15;
        protected override int CardNumPrefix => Etx.CoinToss ? 34 : 37;
        protected override string CcName => "AMEX";
    }

    [Serializable]
    public class DiscoverCc : CreditCard
    {
        public DiscoverCc(IPerson cardholder, DateTime openedDate, float minPaymentRate, Pecuniam ccMax = null)
            : base(cardholder, openedDate, minPaymentRate, ccMax)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => 6011;
        protected override string CcName => "DC";
    }
}
