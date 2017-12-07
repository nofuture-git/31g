using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp.Cc
{
    /// <summary>
    /// Represents a personal credit card in the form of 
    /// both its properties (e.g. owner, expiry, etc) and
    /// the history of transactions and payments.
    /// </summary>
    [Serializable]
    public class CreditCardAccount : FixedRateLoan, IAccount<Identifier>
    {
        #region constants
        public const float DF_MIN_PMT_RATE = 0.0125F;
        #endregion

        #region ctor
        public CreditCardAccount(ICreditCard cc, float minPaymentRate, Pecuniam ccMax = null)
            : base(cc.CardHolderSince, minPaymentRate <= 0 ? DF_MIN_PMT_RATE : minPaymentRate)
        {
            Cc = cc;
            base.TradeLine.CreditLimit = ccMax ?? new Pecuniam(1000);
            base.TradeLine.FormOfCredit = FormOfCredit.Revolving;
            base.TradeLine.DueFrequency = new TimeSpan(30, 0, 0, 0);
        }
        #endregion

        #region properties
        public Pecuniam Max => TradeLine.CreditLimit;
        public ICreditCard Cc { get; }
        public Identifier Id => Cc.Number;
        public DateTime? Inception {get { return TradeLine.OpennedDate; } set{ } }

        public DateTime? Terminus
        {
            get => TradeLine.Closure?.ClosedDate;
            set
            {
                if (value.HasValue)
                    TradeLine.Closure = new TradelineClosure {ClosedDate = value.Value};
            }
        }
        #endregion

        #region methods
        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = Inception == null || Inception <= dt;
            var beforeOrOnToDt = Terminus == null || Terminus.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        /// <summary>
        /// Public API method to allow the <see cref="Max"/> to 
        /// be increased and only increased.
        /// </summary>
        /// <param name="val"></param>
        public void IncreaseMaxTo(Pecuniam val)
        {
            if (Max != null && Max > val)
                return;
            base.TradeLine.CreditLimit = val;
        }

        /// <summary>
        /// Asserts that the current balance equals-or-exceeds
        /// this instances <see cref="Max"/>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool IsMaxedOut(DateTime dt)
        {
            return GetValueAt(dt) >= Max;
        }

        /// <summary>
        /// Applies a purchase transation to this credit card.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="val"></param>
        /// <param name="note"></param>
        /// <param name="fee"></param>
        /// <returns>
        /// True when the card is not expired and
        /// the purchase amount <see cref="val"/>
        /// will not cause the total balance to exceed <see cref="Max"/>.
        /// </returns>
        public override bool Pop(DateTime dt, Pecuniam val, IMereo note = null, Pecuniam fee = null)
        {
            if (dt > Cc.ExpDate)
                return false;
            var cBal = GetValueAt(dt);
            if (cBal >= Max || cBal + val >= Max)
                return false;
            return base.Pop(dt, val, note, fee);
        }

        /// <summary>
        /// Returns the credit card in a format
        /// like what is on a receipt.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Cc?.ToString() ?? base.ToString();
        }

        /// <summary>
        /// Randomly gen's one of the concrete types of <see cref="CreditCardAccount"/>.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ccScore">
        /// Optional, if given then will generate an interest-rate and cc-max 
        /// in accordance with the score.
        /// </param>
        /// <param name="baseInterestRate">
        /// This is the lowest possiable interest rate for the random generators
        /// </param>
        /// <param name="minPmtPercent">
        /// The value used to calc a minimum monthly payment
        /// </param>
        /// <returns></returns>
        public static CreditCardAccount GetRandomCcAcct(IPerson p, CreditScore ccScore,
            float baseInterestRate = 10.1F + Gov.Fed.RiskFreeInterestRate.DF_VALUE,
            float minPmtPercent = DF_MIN_PMT_RATE)
        {
            if(ccScore == null && p is NorthAmerican)
                ccScore = new PersonalCreditScore((NorthAmerican) p);

            var cc = CreditCard.GetRandomCreditCard(p);
            var max = ccScore == null ? new Pecuniam(1000) : ccScore.GetRandomMax(cc.CardHolderSince);
            var randRate = ccScore?.GetRandomInterestRate(cc.CardHolderSince, baseInterestRate) * 0.01 ?? baseInterestRate;
            var ccAcct = new CreditCardAccount(cc, minPmtPercent, max) {Rate = (float) randRate};
            return ccAcct;
        }
        #endregion
    }
}