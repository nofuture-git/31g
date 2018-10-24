using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp.Cc
{
    [Serializable]
    public class VisaCc : CreditCard
    {
        public VisaCc(){ }
        public VisaCc(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }
        public VisaCc(string cardNumber, IVoca cardholder, DateTime? openedDate, DateTime? expiryDate) : base(
            cardNumber, cardholder, openedDate, expiryDate)
        {
        }

        public VisaCc(string cardNumber, string cvv, IVoca cardholder, DateTime? openedDate,
            DateTime? expiryDate) : base(cardNumber, cvv, cardholder, openedDate, expiryDate)
        {
        }
        protected internal override int CardNumLen => 16;
        protected internal override Rchar[] CardNumPrefix => new[] {new RcharLimited(0, '4')};

        public override string CcName => "VISA";

        /// <summary>
        /// Gets a Visa credit card number at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CreditCardNumber RandomVisaNumber()
        {
            var card = new VisaCc();
            return card.GetRandomCardNumber();
        }

        /// <summary>
        /// Affirms if the <see cref="cardNumber"/> matches the pattern.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static bool IsValidNumber(string cardNumber)
        {
            return new VisaCc().Number.Validate(cardNumber);
        }
    }
}