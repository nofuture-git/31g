using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="TradeLine" />
    /// <inheritdoc cref="IReceivable" />
    /// <summary>
    /// </summary>
    [Serializable]
    public abstract class Receivable : TradeLine, IReceivable
    {
        private readonly IMereo _expectation = new Mereo();
        #region ctor
        protected Receivable(DateTime openedDate):base(openedDate)
        {
        }
        protected Receivable(){ }
        #endregion

        #region properties

        public SpStatus? CurrentStatus
        {
            get
            {
                if (DueFrequency == null)
                    return null;
                return GetStatus(DateTime.Now);
            }
        }
        public PastDue? CurrentDelinquency => GetDelinquency(DateTime.Now);
        public virtual Pecuniam Value => Balance.GetCurrent(DateTime.Now, 0f);
        public IMereo Expectation => _expectation;

        #endregion

        #region methods
        public virtual PastDue? GetDelinquency(DateTime dt)
        {
            if (GetStatus(dt) != SpStatus.Late)
                return null;

            //30 days past due when last payment was normal billing cycle plus 30 days
            var billingCycleDays = (DueFrequency ?? DefaultDueFrequency).TotalDays;

            var justLate = new Tuple<DateTime, DateTime>(dt.AddDays(-29 - billingCycleDays),
                dt.AddDays(billingCycleDays*-1));

            if ((Balance.GetDebitSum(justLate)).Amount < 0)
                return null;

            //the line was openned some time before 30DPD
            if (DateTime.Compare(Inception, dt.AddDays(-30 - billingCycleDays)) > 0)
                return null;

            var thirtyDpd = new Tuple<DateTime, DateTime>(dt.AddDays(-59 - billingCycleDays),
                dt.AddDays(-30 - billingCycleDays));

            if ((Balance.GetDebitSum(thirtyDpd)).Amount < 0)
                return PastDue.Thirty;

            if (DateTime.Compare(Inception, dt.AddDays(-60 - billingCycleDays)) > 1)
                return PastDue.Thirty;

            var sixtyDpd = new Tuple<DateTime, DateTime>(dt.AddDays(-89 - billingCycleDays),
                dt.AddDays(-60 - billingCycleDays));

            if ((Balance.GetDebitSum(sixtyDpd)).Amount < 0)
                return PastDue.Sixty;

            if (DateTime.Compare(Inception, dt.AddDays(-90 - billingCycleDays)) > 1)
                return PastDue.Sixty;

            var nintyDpd = new Tuple<DateTime, DateTime>(dt.AddDays(-179 - billingCycleDays),
                dt.AddDays(-90 - billingCycleDays));

            if ((Balance.GetDebitSum(nintyDpd)).Amount < 0)
                return PastDue.Ninety;

            if (DateTime.Compare(Inception, dt.AddDays(-180 - billingCycleDays)) > 1)
                return PastDue.Ninety;

            return PastDue.HundredAndEighty;
        }

        public virtual SpStatus GetStatus(DateTime? dt)
        {
            var ddt = dt ?? DateTime.Now;
            if (Terminus != null && DateTime.Compare(Terminus.Value, ddt) < 0)
                return SpStatus.Closed;

            if (Balance.IsEmpty)
                return SpStatus.NoHistory;

            //make sure something is actually owed
            if (GetValueAt(ddt).Amount <= 0)
                return SpStatus.Current;

            var lastPayment =
                Balance.GetDebitSum(
                    new Tuple<DateTime, DateTime>(ddt.AddDays((DueFrequency ?? DefaultDueFrequency).TotalDays * -1),
                        ddt));

            return lastPayment.GetAbs() < GetMinPayment(ddt).GetAbs()
                ? SpStatus.Late
                : SpStatus.Current;
        }

        public virtual Pecuniam GetValueAt(DateTime dt)
        {
            return Balance.GetCurrent(dt, 0.0F);
        }

        public virtual Pecuniam GetMinPayment(DateTime dt)
        {
            return Pecuniam.Zero;
        }

        public override IDictionary<string, object> ToData(KindsOfTextCase txtCase)
        {
            Func<string, string> textFormat = (x) => VocaBase.TransformText(x, txtCase);
            var itemData = base.ToData(txtCase) ?? new Dictionary<string, object>();

            if(Expectation != null && Expectation.Value != Pecuniam.Zero)
                itemData.Add(textFormat("ExpectValue"), Expectation.Value.ToString());

            var v = Value;
            if(v != Pecuniam.Zero)
                itemData.Add(textFormat("ActualValue"), v.ToString());

            var status = CurrentStatus;
            if(status != null)
                itemData.Add(textFormat("Status"), status.ToString());

            var delq = CurrentDelinquency;
            if(delq != null)
                itemData.Add(textFormat(nameof(PastDue)), delq.ToString());

            return itemData;
        }

        #endregion  
    }
}