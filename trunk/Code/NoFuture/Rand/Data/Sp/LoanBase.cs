using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public abstract class LoanBase<T> : Pondus, ILoan
    {
        private float _minPaymentRate;

        #region ctors
        protected LoanBase(DateTime openedDate, float minPaymentRate):base(openedDate)
        {
            _minPaymentRate = minPaymentRate;
        }
        #endregion

        #region properties
        public virtual float MinPaymentRate
        {
            get => _minPaymentRate;
            set => _minPaymentRate = value;
        }
        public virtual T Rate { get; set; }
        #endregion

        #region methods
        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var bal = GetValueAt(dt);
            if (bal < Pecuniam.Zero)
                return Pecuniam.Zero;

            var amt = bal.Amount * Convert.ToDecimal(MinPaymentRate);
            return new Pecuniam(Math.Round(amt, 2)).Neg;
        }

        #endregion
    }
}