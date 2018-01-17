using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp.Cc
{
    [Serializable]
    public class MasterCardCc : CreditCard
    {
        public MasterCardCc(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        protected internal override int CardNumLen => 16;
        protected internal override int CardNumPrefix => Etx.RandomInteger(51, 55);
        protected internal override string CcName => "MC";

        /// <summary>
        /// Gets a MasterCard credit card number at random
        /// </summary>
        /// <returns></returns>
        [RandomFactory]
        public static CreditCardNumber RandomMasterCardNumber()
        {
            var card = new MasterCardCc(null, null, null);
            return card.GetRandomCardNumber();
        }
    }
}