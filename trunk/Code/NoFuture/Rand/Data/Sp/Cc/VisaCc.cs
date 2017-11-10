using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp.Cc
{
    [Serializable]
    public class VisaCc : CreditCard
    {
        public VisaCc(IVoca cardholder, DateTime? openedDate, DateTime? expiryDate)
            : base(cardholder, openedDate, expiryDate)
        {
        }

        protected override int CardNumLen => 16;
        protected override int CardNumPrefix => 4;
        protected override string CcName => "VISA";
    }
}