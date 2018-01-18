using System;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Shared.Core;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class FixedRateLoan : LoanBase<float>
    {
        public FixedRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amount = null)
            : base(openedDate, minPaymentRate, amount)
        {
        }

        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, Rate);
        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, Rate);
        }
    }
}