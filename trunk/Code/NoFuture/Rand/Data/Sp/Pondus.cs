using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Data.Sp
{
    /// <inheritdoc cref="Mereo" />
    /// <summary>
    /// A composition type to bind a time-range and an income money amount to a name
    /// </summary>
    [Serializable]
    public class Pondus : Mereo, IIdentifier<Pecuniam>, ITempore
    {
        private Tuple<DateTime?, DateTime?> _dateRange;

        public Pondus(string name) : base(name)
        {
        }

        public Pondus(IVoca names) : base(names)
        {
        }

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
    }
}
