using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;

namespace NoFuture.Rand.Data.Sp //Sequere pecuniam
{
    [Flags]
    [Serializable]
    public enum FormOfCredit : short
    {
        None = 0,
        Revolving = 1,
        Installment = 2,
        Mortgage = 4,
        Fixed = 8,
    }

    public interface ILoan
    {
        /// <summary>
        /// Can be set to a combination of the <see cref="FormOfCredit"/>
        /// </summary>
        FormOfCredit KindOfLoan { get; set; }

        /// <summary>
        /// The rate used to calc the minimum payment.
        /// </summary>
        float MinPaymentRate { get; set; }

        /// <summary>
        /// Credit history report for this loan
        /// </summary>
        TradeLine TradeLine { get;}

        IFirm Lender { get; set; }
    }

    [Serializable]
    public abstract class LoanBase<T> : ReceivableBase, ILoan
    {
        #region ctors
        protected LoanBase(DateTime openedDate, float minPaymentRate):base(openedDate)
        {
            MinPaymentRate = minPaymentRate;
        }
        #endregion

        #region properties
        public FormOfCredit KindOfLoan { get; set; }
        public float MinPaymentRate { get; set; }
        public T Rate { get; set; }
        public IFirm Lender { get; set; }
        #endregion

        #region methods
        public override Pecuniam GetMinPayment(DateTime dt)
        {
            var bal = GetCurrentBalance(dt);
            if (bal < Pecuniam.Zero)
                return Pecuniam.Zero;

            var amt = bal.Amount * Convert.ToDecimal(MinPaymentRate);
            return new Pecuniam(Math.Round(amt, 2)).Neg;
        }
        public void MakeAPayemnt(DateTime dt, Pecuniam amt)
        {
            if (amt == Pecuniam.Zero)
                return;
            while (TradeLine.Balance.Transactions.Any(x => DateTime.Compare(x.AtTime, dt) == 0))
                dt = dt.AddMilliseconds(10);
            TradeLine.Balance.Transactions.Add(new Transaction(dt, amt.Neg));
        }
        #endregion
    }

    [Serializable]
    public class FixedRateLoan : LoanBase<float>
    {
        #region ctors

        public FixedRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amt = null) : base(openedDate, minPaymentRate)
        {
            if(amt != null && amt.Amount != 0)
                _tl.Balance.Transactions.Add(new Transaction(openedDate, amt.Abs));
        }
        #endregion

        #region methods
        public override Pecuniam GetCurrentBalance(DateTime dt)
        {
            return TradeLine.Balance.GetCurrent(dt, Rate);
        }
        #endregion
    }

    [Serializable]
    public class VariableRateLoan : LoanBase<Dictionary<DateTime, float>>
    {
        #region ctors

        public VariableRateLoan(DateTime openedDate, float minPaymentRate, Pecuniam amt = null) : base(openedDate, minPaymentRate)
        {
            if (amt != null && amt.Amount != 0)
                _tl.Balance.Transactions.Add(new Transaction(openedDate, amt.Abs));
        }
        #endregion

        #region methods
        public override Pecuniam GetCurrentBalance(DateTime dt)
        {
            return TradeLine.Balance.GetCurrent(dt, Rate);
        }
        #endregion
    }
}
