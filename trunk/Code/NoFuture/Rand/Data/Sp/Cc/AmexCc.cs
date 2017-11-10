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

        protected override int CardNumLen => 15;
        protected override int CardNumPrefix => Etx.CoinToss ? 34 : 37;
        protected override string CcName => "AMEX";
    }
}