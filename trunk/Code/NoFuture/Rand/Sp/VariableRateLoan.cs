using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// A loan whose rate changes in time
    /// </summary>
    [Serializable]
    public class VariableRateLoan : LoanBase<Dictionary<DateTime, float>>
    {
        #region ctors

        public VariableRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amt = null)
            : this(openedDate, amt)
        {
            _minPaymentRate = minPaymentRate;
        }

        public VariableRateLoan(DateTime openedDate, Pecuniam amount) : base(openedDate, amount)
        {
            if (amount != null && amount.Amount != 0)
                Balance.AddPositiveValue(openedDate, amount.GetAbs(), new TransactionNote("Initial Transaction"));
        }

        #endregion

        public override Pecuniam Value => Balance.GetCurrent(DateTime.UtcNow, Rate);

        #region methods
        public override Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, Rate);
        }
        #endregion
    }
}