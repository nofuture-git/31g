using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp.Cc
{
    [Serializable]
    public class AmexCc : CreditCard
    {
        public AmexCc(){ }
        public AmexCc(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        public AmexCc(string cardNumber, IVoca cardholder, DateTime? openedDate, DateTime? expiryDate) : base(
            cardNumber, cardholder, openedDate, expiryDate)
        {
        }

        public AmexCc(string cardNumber, string cvv, IVoca cardholder, DateTime? openedDate,
            DateTime? expiryDate) : base(cardNumber, cvv, cardholder, openedDate, expiryDate)
        {
        }

        protected internal override int CardNumLen => 15;

        protected internal override Rchar[] CardNumPrefix =>
            new[]
            {
                new RcharLimited(0, '3'),
                new RcharLimited(1, '4', '7')
            };

        public override string CcName => "AMEX";

        /// <summary>
        /// Gets a Amex credit card number at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CreditCardNumber RandomAmexNumber()
        {
            var card = new AmexCc();
            return card.GetRandomCardNumber();
        }

        /// <summary>
        /// Affirms if the <see cref="cardNumber"/> matches the pattern.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static bool IsValidNumber(string cardNumber)
        {
            return new AmexCc().Number.Validate(cardNumber);
        }
    }
}