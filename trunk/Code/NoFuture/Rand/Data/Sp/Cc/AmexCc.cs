using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp.Cc
{
    [Serializable]
    public class AmexCc : CreditCard
    {
        public AmexCc(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        protected internal override int CardNumLen => 15;
        protected internal override int CardNumPrefix => Etx.RandomCoinToss() ? 34 : 37;
        protected internal override string CcName => "AMEX";

        /// <summary>
        /// Gets a Amex credit card number at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CreditCardNumber RandomAmexNumber()
        {
            var card = new AmexCc(null, null, null);
            return card.GetRandomCardNumber();
        }
    }
}