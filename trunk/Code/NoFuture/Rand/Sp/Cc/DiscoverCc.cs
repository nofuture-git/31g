using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp.Cc
{
    [Serializable]
    public class DiscoverCc : CreditCard
    {
        public DiscoverCc() { }
        public DiscoverCc(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }
        public DiscoverCc(string cardNumber, IVoca cardholder, DateTime? openedDate, DateTime? expiryDate) : base(
            cardNumber, cardholder, openedDate, expiryDate)
        {
        }

        public DiscoverCc(string cardNumber, string cvv, IVoca cardholder, DateTime? openedDate,
            DateTime? expiryDate) : base(cardNumber, cvv, cardholder, openedDate, expiryDate)
        {
        }
        protected internal override int CardNumLen => 16;

        protected internal override Rchar[] CardNumPrefix =>
            new[]
            {
                new RcharLimited(0, '6'),
                new RcharLimited(1, '0'),
                new RcharLimited(2, '1'),
                new RcharLimited(3, '1')
            };

        public override string CcName => "DC";

        /// <summary>
        /// Gets a Discover credit card number at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CreditCardNumber RandomDiscoverNumber()
        {
            var card = new DiscoverCc();
            return card.GetRandomCardNumber();
        }

        /// <summary>
        /// Affirms if the <see cref="cardNumber"/> matches the pattern.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static bool IsValidNumber(string cardNumber)
        {
            return new DiscoverCc().Number.Validate(cardNumber);
        }
    }
}