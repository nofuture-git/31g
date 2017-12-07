﻿using System;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public abstract class LoanBase<T> : ReceivableBase, ILoan
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
        public IFirm Lender { get; set; }
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