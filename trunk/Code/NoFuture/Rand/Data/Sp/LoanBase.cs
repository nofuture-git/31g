using System;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public abstract class LoanBase<T> : Pondus, ILoan
    {
        protected float _minPaymentRate;

        protected LoanBase(DateTime openedDate, float minPaymentRate, Pecuniam amount = null) :base(openedDate)
        {
            if (amount != null && amount.Amount != 0)
            {
                Balance.AddTransaction(openedDate, amount.Abs, new Mereo("Initial Transaction"), Pecuniam.Zero);
            }

            _minPaymentRate = minPaymentRate;
        }

        /// <summary>
        /// The rate at which the minimum payment is calculated
        /// </summary>
        public virtual float MinPaymentRate
        {
            get => _minPaymentRate;
            set => _minPaymentRate = value;
        }

        /// <summary>
        /// The borrowing rate for the loan.
        /// </summary>
        public virtual T Rate { get; set; }

        /// <summary>
        /// Gets the minimum payment based on the value at time <see cref="dt"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var bal = GetValueAt(dt);
            if (bal < Pecuniam.Zero)
                return Pecuniam.Zero;

            var amt = bal.Amount * Convert.ToDecimal(MinPaymentRate);
            return new Pecuniam(Math.Round(amt, 2)).Neg;
        }

    }
}