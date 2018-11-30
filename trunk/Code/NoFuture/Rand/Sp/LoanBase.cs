using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="ILoan"/>
    /// <inheritdoc cref="NamedReceivable"/>
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class LoanBase<T> : NamedReceivable, ILoan
    {
        public const float DF_MIN_PMT_RATE = 0.0125F;

        protected float _minPaymentRate;

        protected LoanBase(DateTime openedDate, float minPaymentRate, Pecuniam amount = null) :this(openedDate, amount)
        {
            _minPaymentRate = minPaymentRate;
        }

        protected LoanBase(DateTime openedDate, Pecuniam amount) : base(openedDate)
        {
            if (amount != null && amount.Amount != 0)
            {
                Balance.AddPositiveValue(openedDate, amount.GetAbs(), new TransactionNote("Initial Transaction"));
            }
            FormOfCredit = Enums.FormOfCredit.Installment;
            DueFrequency = DefaultDueFrequency;
            _minPaymentRate = DF_MIN_PMT_RATE;
        }

        public virtual float MinPaymentRate
        {
            get => _minPaymentRate;
            set => _minPaymentRate = value;
        }

        /// <summary>
        /// The borrowing rate for the loan.
        /// </summary>
        public virtual T Rate { get; set; }

        public virtual Pecuniam OriginalBorrowAmount => Balance.FirstTransaction.Cash;

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
            return new Pecuniam(Math.Round(amt, 2)).GetNeg();
        }

        /// <summary>
        /// Helper method to put functionality in common vernacular 
        /// - is the exact same as <see cref="ITransactionable.AddNegativeValue"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="amount"></param>
        /// <param name="note"></param>
        public virtual void MakeAPayment(DateTime dt, Pecuniam amount, IVoca note = null)
        {
            if (note == null)
                AddNegativeValue(dt, amount);
            else
                AddNegativeValue(dt, amount, note);
        }
    }
}