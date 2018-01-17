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

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => Etx.RandomInteger(51, 55);
        protected override string CcName => "MC";
    }
}