using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp.Cc
{
    [Serializable]
    public class VisaCc : CreditCard
    {
        public VisaCc(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        protected internal override int CardNumLen => 16;
        protected internal override int CardNumPrefix => 4;
        public override string CcName => "VISA";

        /// <summary>
        /// Gets a Visa credit card number at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CreditCardNumber RandomVisaNumber()
        {
            var card = new VisaCc(null, null, null);
            return card.GetRandomCardNumber();
        }
    }
}