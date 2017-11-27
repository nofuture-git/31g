using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp
{
    /// <inheritdoc cref="Mereo" />
    /// <summary>
    /// A composition type to bind a time-range and an money amount to a name
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
            get => _dateRange?.Item1;
            set => _dateRange = new Tuple<DateTime?, DateTime?>(value, _dateRange?.Item2);
        }

        public virtual DateTime? ToDate
        {
            get => _dateRange?.Item2;
            set => _dateRange = new Tuple<DateTime?, DateTime?>(_dateRange?.Item1, value);
        }

        public virtual bool IsInRange(DateTime dt)
        {
            var afterOrOnFromDt = FromDate == null || FromDate <= dt;
            var beforeOrOnToDt = ToDate == null || ToDate.Value >= dt;
            return afterOrOnFromDt && beforeOrOnToDt;
        }

        public static Pecuniam operator +(Pondus a, Pondus b)
        {
            var ap = a?.Value ?? Pecuniam.Zero;
            var bp = b?.Value ?? Pecuniam.Zero;
            return ap + bp;
        }

        public static Pecuniam operator -(Pondus a, Pondus b)
        {
            var ap = a?.Value ?? Pecuniam.Zero;
            var bp = b?.Value ?? Pecuniam.Zero;
            return ap - bp;
        }

        public static Pecuniam GetSum(IEnumerable<Pondus> items)
        {
            if(items == null || !items.Any())
                return Pecuniam.Zero;

            var p = Pecuniam.Zero;
            foreach (var i in items)
                p += i?.Value ?? Pecuniam.Zero;
            return p;
        }

        /// <summary>
        /// Helper method to get an annual sum based on each items interval
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Pecuniam GetAnnualSum(IEnumerable<Pondus> items)
        {
            var sum = 0M;
            foreach (var item in items)
            {
                if (item?.Value == null ||
                    !NAmerUtil.Tables.Interval2AnnualPayMultiplier.ContainsKey(item.Interval))
                    continue;
                sum += item.Value.Amount * NAmerUtil.Tables.Interval2AnnualPayMultiplier[item.Interval];
            }
            return new Pecuniam(sum);
        }

        /// <summary>
        /// Consider equality as being the same name at the same time
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //asserts the names equal
            var baseEq = base.Equals(obj);
            var p = obj as Pondus;

            if (p == null || !baseEq)
                return base.Equals(obj);

            var sDtEq = FromDate == p.FromDate;
            var eDtEq = ToDate == p.ToDate;

            return sDtEq && eDtEq;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + _dateRange?.GetHashCode() ?? 1;
        }
    }
}
