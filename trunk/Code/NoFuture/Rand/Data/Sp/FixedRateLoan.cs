using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public class FixedRateLoan : LoanBase<float>
    {
        #region ctors

        public FixedRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(openedDate, minPaymentRate)
        {
            if (amt != null && amt.Amount != 0)
                Balance.AddTransaction(openedDate, amt.Abs, new Mereo("Initial Transaction"), Pecuniam.Zero);
        }

        #endregion

        public override Pecuniam Value => Balance.GetCurrent(DateTime.Now, Rate);

        #region methods
        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, Rate);
        }
        #endregion
    }
}