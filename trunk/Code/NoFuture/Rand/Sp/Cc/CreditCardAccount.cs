using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp.Cc
{
    /// <summary>
    /// Represents a personal credit card in the form of 
    /// both its properties (e.g. owner, expiry, etc) and
    /// the history of transactions and payments.
    /// </summary>
    [Serializable]
    public class CreditCardAccount : FixedRateLoan, IAccount<Identifier>
    {
        #region ctor
        public CreditCardAccount(ICreditCard cc, float minPaymentRate, Pecuniam ccMax = null)
            : base(cc.CardHolderSince, minPaymentRate <= 0 ? DF_MIN_PMT_RATE : minPaymentRate)
        {
            Cc = cc;
            Max = ccMax ?? new Pecuniam(1000);
            FormOfCredit = Rand.Sp.Enums.FormOfCredit.Revolving;
            DueFrequency = new TimeSpan(30, 0, 0, 0);
        }
        #endregion

        #region properties
        public Pecuniam Max { get; private set; }

        public ICreditCard Cc { get; }

        public Identifier Id => Cc.Number;

        public KindsOfAccounts? AccountType => KindsOfAccounts.Liability;

        public bool IsOppositeForm => true;

        #endregion

        #region methods
        /// <summary>
        /// Helper method to put functionality in common vernacular 
        /// - is the exact same as <see cref="AddPositiveValue"/> and <see cref="Credit"/>
        /// </summary>
        public Guid MakePurchase(DateTime dt, Pecuniam val, IVoca note = null)
        {
            return AddPositiveValue(dt, val, note);
        }

        /// <summary>
        /// Helper method to put functionality in common vernacular 
        /// - is the exact same as <see cref="AddPositiveValue"/> and <see cref="Debit"/>
        /// </summary>
        public virtual Guid MakePayment(DateTime dt, Pecuniam val, IVoca note = null)
        {
            return AddNegativeValue(dt, val, note);
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
            Max = val;
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
        /// Applies a purchase transaction to this credit card.
        /// </summary>
        /// <returns>
        /// True when the card is not expired and
        /// the purchase amount <see cref="amount"/>
        /// will not cause the total balance to exceed <see cref="Max"/>.
        /// </returns>
        public override Guid AddPositiveValue(DateTime dt, Pecuniam amount, IVoca note = null, ITransactionId trace = null)
        {
            if (dt > Cc.ExpDate)
                return Guid.Empty;
            var cBal = GetValueAt(dt);
            if (cBal >= Max || cBal + amount >= Max)
                return Guid.Empty;
            return base.AddPositiveValue(dt, amount, note);
        }

        /// <summary>
        /// Applies a payment to the credit card account
        /// </summary>
        /// <returns>
        /// when <see cref="dt"/> is after the expiration date
        /// </returns>
        public override Guid AddNegativeValue(DateTime dt, Pecuniam amount, IVoca note = null, ITransactionId trace = null)
        {
            if (dt > Cc.ExpDate)
                return Guid.Empty;
            return base.AddNegativeValue(dt, amount, note, trace);
        }

        /// <summary>
        /// This is the Accounting name for making a payment (<see cref="MakePayment"/>)
        /// </summary>
        public IAccount<Identifier> Debit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? Balance.LastTransaction.AtTime;
            AddNegativeValue(dt, amt, note, trace);
            return this;
        }

        /// <summary>
        /// This is the Accounting name for making a purchase (<see cref="MakePurchase"/>
        /// </summary>
        public IAccount<Identifier> Credit(Pecuniam amt, IVoca note = null, DateTime? atTime = null, ITransactionId trace = null)
        {
            var dt = atTime ?? Balance.LastTransaction.AtTime;
            AddPositiveValue(dt, amt, note, trace);
            return this;
        }

        public bool AnyTransaction(Predicate<ITransactionId> filter)
        {
            return Balance.AnyTransaction(filter);
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

        #endregion
    }
}