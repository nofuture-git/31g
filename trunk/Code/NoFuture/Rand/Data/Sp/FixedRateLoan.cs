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
                _tl.Balance.AddTransaction(openedDate, amt.Abs, Pecuniam.Zero, Domus.Opes.WealthBase.GetPaymentNote(null, "Initial Transaction"));
        }

        #endregion

        #region methods
        public override Pecuniam GetValueAt(DateTime dt)
        {
            return TradeLine.Balance.GetCurrent(dt, Rate);
        }
        #endregion
    }
}