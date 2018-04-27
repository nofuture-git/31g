using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Sp
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
                Balance.AddTransaction(openedDate, amt.GetAbs(), new Mereo("Initial Transaction"), Pecuniam.Zero);
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