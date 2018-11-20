using System;

namespace NoFuture.Rand.Sp
{
    [Serializable]
    public class FixedRateLoan : LoanBase<float>
    {
        public FixedRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amount = null)
            : base(openedDate, minPaymentRate, amount)
        {
        }

        public override Pecuniam Value => Balance.GetCurrent(DateTime.UtcNow, Rate);
        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, Rate);
        }
    }
}