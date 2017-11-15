using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Domus.Opes
{
    /// <summary>
    /// A composition type to bind a time-range, a money amount and a descriptions together.
    /// </summary>
    [Serializable]
    public class Pondus : IIdentifier<Pecuniam>, ITempore
    {
        private Tuple<DateTime?, DateTime?> _dateRange;

        public IMereo Description { get; set; }
        public IncomeInterval Interval { get; set; }
        public string Src { get; set; }
        public string Abbrev => Description?.GetName(KindsOfNames.Abbrev);
        public Pecuniam Value { get; set; }

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

        public virtual bool Equals(Pondus p)
        {
            if (p == null)
                return false;
            return p.Value == Value
                   && p.Description.Equals(Description)
                   && p.FromDate == FromDate
                   && p.ToDate == ToDate;
        }

        public override bool Equals(object obj)
        {
            var p = obj as Pondus;
            return p == null ? base.Equals(obj) : Equals(p);
        }

        public override int GetHashCode()
        {
            return Description?.GetHashCode() ?? 1 +
                   Value?.GetHashCode() ?? 1 +
                   _dateRange?.GetHashCode() ?? 1;
        }
    }
}
