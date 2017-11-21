using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Domus.Opes
{
    /// <inheritdoc cref="IEmployment" />
    /// <inheritdoc cref="WealthBase" />
    /// <summary>
    /// </summary>
    [Serializable]
    public class NorthAmericanEmployment : WealthBase, IEmployment
    {
        #region fields
        private Tuple<DateTime?, DateTime?> _dateRange;
        private readonly HashSet<Pondus> _pay = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _deductions = new HashSet<Pondus>();
        private Interval _interval;
        private SocDetailedOccupation _occupation;
        private bool _isWages;
        private bool _isTips;
        private bool _isCommission;
        #endregion

        #region ctors

        public NorthAmericanEmployment(DateTime? startDate, DateTime? endDate, NorthAmerican american) : base(american)
        {
            _dateRange = new Tuple<DateTime?, DateTime?>(startDate, endDate);
            Occupation = StandardOccupationalClassification.RandomOccupation();
            
        }

        #endregion

        #region properties
        public virtual string Src { get; set; }
        public virtual string Abbrev => "Employer";
        public virtual IFirm Value { get; set; }
        public virtual bool IsOwner { get; set; }

        public virtual SocDetailedOccupation Occupation
        {
            get => _occupation;
            set
            {
                _occupation = value;
                _isWages = StandardOccupationalClassification.IsWages(_occupation);
                _isTips = StandardOccupationalClassification.IsTips(_occupation);
                _isCommission = StandardOccupationalClassification.IsCommissions(_occupation);
                _interval = _isWages || _isTips || _isCommission ? Interval.Weekly : Interval.BiWeekly;
            }
        }

        public virtual Pondus[] CurrentDeductions => GetDeductionsAt(null);
        public virtual Pondus CurrentPay => GetPayAt(null);

        public virtual DateTime? FromDate
        {
            get => _dateRange.Item1;
            set => _dateRange = new Tuple<DateTime?, DateTime?>(value, _dateRange.Item2);
        }

        public virtual DateTime? ToDate
        {
            get => _dateRange.Item2;
            set => _dateRange = new Tuple<DateTime?, DateTime?>(_dateRange.Item1, value);
        }

        public Pecuniam CurrentNetPay => (CurrentPay?.Value ?? Pecuniam.Zero) - Pondus.GetSum(CurrentDeductions).Abs;

        #endregion

        #region methods

        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = FromDate == null || FromDate <= dt;
            var beforeOrOnToDt = ToDate == null || ToDate.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public virtual Pondus GetPayAt(DateTime? dt)
        {
            var pay = GetAt(dt, Pay);
            return pay.LastOrDefault();
        }

        public virtual Pondus[] GetDeductionsAt(DateTime? dt)
        {
            return GetAt(dt, Deductions);
        }

        protected internal virtual void ResolvePay()
        {
            //do we need to spread multiple pay items across years
            var hasStartDate = _dateRange?.Item1 != null;

            

            throw new NotImplementedException();
        }

        protected internal virtual void ResolveDeductions()
        {
            throw new NotImplementedException();
        }

        protected internal virtual List<Tuple<DateTime, DateTime>> GetYearsOfServiceInDates()
        {
            var ranges = new List<Tuple<DateTime, DateTime>>();

            var yearsOfService = GetYearsOfService();
            if (yearsOfService <= 0 || _dateRange?.Item1 == null)
                return ranges;

            var employerFiscalYearEnd = Value?.FiscalYearEndDay ?? 1;

            //assume merit increases 90 days after fiscal year end
            var annualReviewDays = employerFiscalYearEnd + 90;
            var maxEndDt = _dateRange.Item2.GetValueOrDefault(DateTime.Today);
            var prevDt = _dateRange.Item1.Value.Date;
            for (var i = 0; i <= yearsOfService; i++)
            {
                var stDt = i == 0 ? prevDt : prevDt.AddDays(1);
                var endDt = new DateTime(stDt.AddYears(1).Year, 1, 1).AddDays(annualReviewDays);
                while((endDt - stDt).Days < 365)
                    endDt = new DateTime(endDt.AddYears(1).Year, 1, 1).AddDays(annualReviewDays);

                //determine if we have past the end date
                var isPastMax = endDt > maxEndDt;

                var endDt2 = isPastMax ? maxEndDt : endDt;

                var dtRng = new Tuple<DateTime, DateTime>(stDt, endDt2);
                
                ranges.Add(dtRng);

                if (isPastMax)
                    return ranges;

                prevDt = endDt;
            }
            return ranges;
        }

        protected internal virtual int GetYearsOfService()
        {
            var hasStartDate = _dateRange?.Item1 != null;
            if (!hasStartDate)
                return 0;

            var stDt = _dateRange.Item1.Value;
            var endDt = _dateRange.Item2.GetValueOrDefault(DateTime.Today);

            var numYears = endDt.Year - stDt.Year;
            return numYears <= 0 ? 0 : numYears;
        }

        protected internal virtual List<Pondus> Deductions
        {
            get
            {
                var d = _deductions.ToList();
                d.Sort(Comparer);
                return d;
            }
        }

        protected internal virtual List<Pondus> Pay
        {
            get
            {
                var p = _pay.ToList();
                p.Sort(Comparer);
                return p;
            }
        }

        protected internal virtual Pondus[] GetPayItemsForRange(Pecuniam amt, DateTime? startDate, DateTime? endDate = null)
        {
            var itemsout = new List<Pondus>();

            //TODO want to have this come in with just the dollar amount
            //TODO   and a date range and come out as a manifold of Pondus

            var incomeItems = GetIncomeItemNames();
            foreach (var incomeItem in incomeItems)
            {
            }

            throw new NotImplementedException();
        }

        protected internal virtual void AddPay(Pecuniam amt, string name, DateTime? startDate, DateTime? endDate = null)
        {
            if (amt == null)
                return;

            var p = new Pondus(name)
            {
                Value = amt.Abs,
                FromDate = startDate ?? _dateRange?.Item1,
                ToDate = endDate ?? _dateRange?.Item2,
                Interval = _interval
            };
            if(IsInRange(p))
                _pay.Add(p);
        }

        protected internal virtual void AddDeduction(Pecuniam amt, string name, DateTime? startDate,
            DateTime? endDate = null)
        {
            if (amt == null)
                return;

            var d = new Pondus(name)
            {
                Value = amt?.Neg,
                FromDate = startDate ?? _dateRange?.Item1,
                ToDate = endDate ?? _dateRange?.Item2,
                Interval = _interval
            };
            if(IsInRange(d))
                _deductions.Add(d);
        }

        protected internal bool IsInRange(Pondus item)
        {
            if (item == null)
                return false;
            var itemEndDt = item.ToDate;
            var rangeStartDt = _dateRange?.Item1;
            
            //item ended before this instance even started
            if (itemEndDt != null && rangeStartDt != null && itemEndDt.Value < rangeStartDt.Value)
                return false;

            var itemStartDt = item.FromDate;
            var rangeEndDt = _dateRange?.Item2;

            //instance ended before this item even started
            if (itemStartDt != null && rangeEndDt != null && itemStartDt.Value > rangeEndDt.Value)
                return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IEmployment e))
                return base.Equals(obj);

            return e.Value != null 
                   && Value != null
                   && e.Value.Equals(Value)
                   && e.FromDate == FromDate
                   && e.ToDate == ToDate;
        }

        public override int GetHashCode()
        {
            return (Value?.GetHashCode() ?? 1) +
                   _dateRange?.GetHashCode() ?? 1;
        }

        #endregion
    }
}