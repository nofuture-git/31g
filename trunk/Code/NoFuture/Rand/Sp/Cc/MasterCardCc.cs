using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp.Cc
{
    [Serializable]
    public class MasterCardCc : CreditCard
    {
        public MasterCardCc(){ }
        public MasterCardCc(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }
        public MasterCardCc(string cardNumber, IVoca cardholder, DateTime? openedDate, DateTime? expiryDate) : base(
            cardNumber, cardholder, openedDate, expiryDate)
        {
        }

        public MasterCardCc(string cardNumber, string cvv, IVoca cardholder, DateTime? openedDate,
            DateTime? expiryDate) : base(cardNumber, cvv, cardholder, openedDate, expiryDate)
        {
        }
        protected internal override int CardNumLen => 16;

        protected internal override Rchar[] CardNumPrefix =>
            new[]
            {
                new RcharLimited(0, '5'),
                new RcharLimited(1, '1', '2', '3', '4', '5')
            };

        public override string CcName => "MC";

        /// <summary>
        /// Gets a MasterCard credit card number at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CreditCardNumber RandomMasterCardNumber()
        {
            var card = new MasterCardCc();
            return card.GetRandomCardNumber();
        }

        /// <summary>
        /// Affirms if the <see cref="cardNumber"/> matches the pattern.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static bool IsValidNumber(string cardNumber)
        {
            return new MasterCardCc().Number.Validate(cardNumber);
        }
    }
}