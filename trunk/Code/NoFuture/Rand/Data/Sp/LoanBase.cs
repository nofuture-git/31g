using System;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public abstract class LoanBase<T> : ReceivableBase, ILoan, ITransactionable 
    {
        #region ctors
        protected LoanBase(DateTime openedDate, float minPaymentRate):base(openedDate)
        {
            MinPaymentRate = minPaymentRate;
        }
        #endregion

        #region properties
        public float MinPaymentRate { get; set; }
        public T Rate { get; set; }
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

        /// <summary>
        /// Applied a negative valued transaction against the balance.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="amt"></param>
        /// <param name="note"></param>
        /// <param name="fee"></param>
        public void Push(DateTime dt, Pecuniam amt, IMereo note = null, Pecuniam fee = null)
        {
            if (amt == Pecuniam.Zero)
                return;
            fee = fee == null ? Pecuniam.Zero : fee.Neg;
            TradeLine.Balance.AddTransaction(dt, amt.Neg, note, fee);
        }

        /// <summary>
        /// Applies a positive valued transaction against the balance
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="val"></param>
        /// <param name="note"></param>
        /// <param name="fee"></param>
        /// <returns></returns>
        public virtual bool Pop(DateTime dt, Pecuniam val, IMereo note = null, Pecuniam fee = null)
        {
            if (val == Pecuniam.Zero)
                return false;
            fee = fee == null ? Pecuniam.Zero : fee.Abs;
            TradeLine.Balance.AddTransaction(dt, val.Abs, note, fee);
            return true;
        }
        #endregion
    }
}