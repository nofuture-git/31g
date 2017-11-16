using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Com;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Domus.Opes
{
    [Serializable]
    public class NorthAmericanEmployment : IEmployment
    {
        private Tuple<DateTime?, DateTime?> _dateRange;
        protected internal IComparer<Pondus> Comparer { get; } = new TemporeComparer();
        private readonly HashSet<Pondus> _pay = new HashSet<Pondus>();
        private readonly HashSet<Pondus> _deductions = new HashSet<Pondus>();

        public NorthAmericanEmployment() {}

        public NorthAmericanEmployment(DateTime? startDate, DateTime? endDate)
        {
            _dateRange = new Tuple<DateTime?, DateTime?>(startDate, endDate);
        }

        public virtual string Src { get; set; }
        public virtual string Abbrev => "Employer";
        public virtual IFirm Value { get; set; }
        public virtual bool IsOwner { get; set; }
        public virtual StandardOccupationalClassification Occupation { get; set; }
        public virtual Pondus[] CurrentDeductions => GetDeductionsAt(null);
        public virtual Pondus CurrentPay => Pay.LastOrDefault();

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

        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = FromDate == null || FromDate <= dt;
            var beforeOrOnToDt = ToDate == null || ToDate.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public virtual Pondus GetPayAt(DateTime? dt)
        {
            return dt == null
                ? CurrentPay
                : Pay.FirstOrDefault(x => x.IsInRange(dt.Value));
        }

        public virtual Pondus[] GetDeductionsAt(DateTime? dt)
        {
            if (dt == null)
            {
                var ld = Deductions.Where(x => x.ToDate == null).ToList();
                ld.Sort(Comparer);
                return ld.ToArray();
            }

            var md = Deductions.Where(d => d.IsInRange(dt.Value)).ToList();
            md.Sort(Comparer);
            return md.ToArray();
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

        protected internal virtual void AddPay(Pecuniam amt, string name, DateTime? startDate, DateTime? endDate = null)
        {
            _pay.Add(new Pondus(name)
            {
                //Description = description,
                Value = amt?.Abs,
                FromDate = startDate,
                ToDate = endDate
            });
        }

        protected internal virtual void AddDeduction(Pecuniam amt, string name, DateTime? startDate,
            DateTime? endDate = null)
        {
            _deductions.Add(new Pondus(name)
            {
                //Description = description,
                Value = amt?.Neg,
                ToDate = endDate,
                FromDate = startDate
            });
        }

        public override bool Equals(object obj)
        {
            var e = obj as IEmployment;
            if (e == null)
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
    }
}