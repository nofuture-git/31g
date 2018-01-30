using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp.Cc
{
    [Serializable]
    public class DiscoverCc : CreditCard
    {
        public DiscoverCc(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        protected internal override int CardNumLen => 16;
        protected internal override int CardNumPrefix => 6011;
        public override string CcName => "DC";

        /// <summary>
        /// Gets a Discover credit card number at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CreditCardNumber RandomDiscoverNumber()
        {
            var card = new DiscoverCc(null, null, null);
            return card.GetRandomCardNumber();
        }
    }
}