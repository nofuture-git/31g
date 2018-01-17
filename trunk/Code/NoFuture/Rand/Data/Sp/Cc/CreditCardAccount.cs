using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Enums;

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

        private Pecuniam _ccMax;

        #region ctor
        public CreditCardAccount(ICreditCard cc, float minPaymentRate, Pecuniam ccMax = null)
            : base(cc.CardHolderSince, minPaymentRate <= 0 ? DF_MIN_PMT_RATE : minPaymentRate)
        {
            Cc = cc;
            _ccMax = ccMax ?? new Pecuniam(1000);
            FormOfCredit = FormOfCredit.Revolving;
            DueFrequency = new TimeSpan(30, 0, 0, 0);
        }
        #endregion

        #region properties
        public Pecuniam Max => _ccMax;
        public ICreditCard Cc { get; }
        public Identifier Id => Cc.Number;
        #endregion

        #region methods

        /// <summary>
        /// Public API method to allow the <see cref="Max"/> to 
        /// be increased and only increased.
        /// </summary>
        /// <param name="val"></param>
        public void IncreaseMaxTo(Pecuniam val)
        {
            if (_ccMax != null && _ccMax > val)
                return;
            _ccMax = val;
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
        /// Randomly gen&apos;s one of the concrete types of <see cref="CreditCardAccount"/>.
        /// </summary>
        /// <param name="p">Card holders name</param>
        /// <param name="personBirthDate">
        /// Optional, will generate a working adults birth date if missing
        /// </param>
        /// <param name="baseInterestRate">
        /// This is the lowest possiable interest rate for the random generators
        /// </param>
        /// <param name="minPmtPercent">
        /// The value used to calc a minimum monthly payment
        /// </param>
        /// <returns></returns>
        public static CreditCardAccount GetRandomCcAcct(IVoca p, DateTime? personBirthDate,
            float baseInterestRate = 10.1F + 1.5F, float minPmtPercent = DF_MIN_PMT_RATE)
        {
            var ccScore =  new PersonalCreditScore(personBirthDate);

            var cc = CreditCard.RandomCreditCard(p);
            var max = ccScore.GetRandomMax(cc.CardHolderSince);
            var randRate = ccScore.GetRandomInterestRate(cc.CardHolderSince, baseInterestRate) * 0.01;
            var ccAcct = new CreditCardAccount(cc, minPmtPercent, max) { Rate = (float)randRate };
            return ccAcct;
        }

        /// <summary>
        /// Randomly gen&apos;s one of the concrete types of <see cref="CreditCardAccount"/>.
        /// </summary>
        /// <param name="p">Card holders name</param>
        /// <param name="ccScore"></param>
        /// <param name="baseInterestRate">
        /// This is the lowest possiable interest rate for the random generators
        /// </param>
        /// <param name="minPmtPercent">
        /// The value used to calc a minimum monthly payment
        /// </param>
        /// <returns></returns>
        public static CreditCardAccount GetRandomCcAcct(IVoca p, PersonalCreditScore ccScore = null,
            float baseInterestRate = 10.1F + 1.5F, float minPmtPercent = DF_MIN_PMT_RATE)
        {
            ccScore = ccScore ?? new PersonalCreditScore();

            var cc = CreditCard.RandomCreditCard(p);
            var max = ccScore.GetRandomMax(cc.CardHolderSince);
            var randRate = ccScore.GetRandomInterestRate(cc.CardHolderSince, baseInterestRate) * 0.01;
            var ccAcct = new CreditCardAccount(cc, minPmtPercent, max) { Rate = (float)randRate };
            return ccAcct;
        }

        #endregion
    }
}