using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class SecuredFixedRateLoan : FixedRateLoan
    {
        public SecuredFixedRateLoan(Identifier property, DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(openedDate, minPaymentRate, amt)
        {
            PropertyId = property;
        }

        public Identifier PropertyId { get; set; }

        #region methods



        #endregion
    }
}