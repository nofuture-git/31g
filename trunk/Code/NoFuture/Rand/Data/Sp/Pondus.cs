using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// A composition type to bind a time-range and an money amount to a name
    /// </summary>
    [Serializable]
    public class Pondus : Receivable, IAccount<IMereo>
    {
        public Pondus(string name)
        {
            Id = new Mereo(name);
        }
        public Pondus(string name, Interval interval)
        {
            Id = new Mereo(name) {Interval = interval};
        }
        public Pondus(IVoca names)
        {
            Id = new Mereo(names);
        }

        public Pondus(IVoca names, Interval interval)
        {
            Id = new Mereo(names) {Interval = interval};
        }

        public Pecuniam ExpectedValue { get; set; }

        public IMereo Id { get; }

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
        public static Pecuniam GetExpectedAnnualSum(IEnumerable<Pondus> items)
        {
            var sum = 0M;
            foreach (var item in items)
            {
                if (item?.ExpectedValue == null ||
                    !NAmerUtil.Tables.Interval2AnnualPayMultiplier.ContainsKey(item.Id.Interval))
                    continue;
                sum += item.ExpectedValue.Amount * NAmerUtil.Tables.Interval2AnnualPayMultiplier[item.Id.Interval];
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
            var p = obj as Pondus;

            if (p == null)
                return base.Equals(obj);
            var namesEqual = p.Id.Equals(Id);

            var sDtEq = Inception == p.Inception;
            var eDtEq = Terminus == p.Terminus;

            return namesEqual && sDtEq && eDtEq;
        }

        public override int GetHashCode()
        {
            return Inception.GetHashCode() + Terminus?.GetHashCode() ?? 1 + Id?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            var d = new Tuple<string, string, string, string, DateTime?, DateTime?>(Value.ToString(), Id.Name,
                Id.GetName(KindsOfNames.Group), Id.Interval.ToString(), Inception, Terminus);
            return d.ToString();
        }
    }
}
