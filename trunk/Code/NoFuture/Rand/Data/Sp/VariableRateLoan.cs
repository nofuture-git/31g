using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// A loan whose rate changes in time
    /// </summary>
    [Serializable]
    public class VariableRateLoan : LoanBase<Dictionary<DateTime, float>>
    {
        #region ctors

        public VariableRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : base(openedDate, minPaymentRate)
        {
            if (amt != null && amt.Amount != 0)
                _tl.Balance.AddTransaction(openedDate, amt.Abs, Pecuniam.Zero, Domus.Opes.WealthBase.GetPaymentNote(null,"Initial Transaction"));
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