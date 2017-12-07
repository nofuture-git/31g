using System;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    [Serializable]
    public abstract class ReceivableBase : TradeLine, IReceivable
    {
        #region ctor
        protected ReceivableBase(DateTime openedDate):base(openedDate)
        {
        }
        #endregion

        #region properties
        //public virtual ITradeLine TradeLine => _tl;
        public SpStatus CurrentStatus => GetStatus(DateTime.Now);
        public PastDue? CurrentDelinquency => GetDelinquency(DateTime.Now);
        public virtual Pecuniam Value => Balance.GetCurrent(DateTime.Now, 0f);

        #endregion

        #region methods
        public virtual PastDue? GetDelinquency(DateTime dt)
        {
            if (GetStatus(dt) != SpStatus.Late)
                return null;

            //30 days past due when last payment was normal billing cycle plus 30 days
            var billingCycleDays = DueFrequency.TotalDays;

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
                    new Tuple<DateTime, DateTime>(ddt.AddDays(DueFrequency.TotalDays*-1), ddt));

            return lastPayment.Abs < GetMinPayment(ddt).Abs
                ? SpStatus.Late
                : SpStatus.Current;
        }

        public abstract Pecuniam GetValueAt(DateTime dt);

        public abstract Pecuniam GetMinPayment(DateTime dt);

        #endregion  
    }
}