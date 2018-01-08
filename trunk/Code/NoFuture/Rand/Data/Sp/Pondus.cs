using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <summary>
    /// A composition type to bind a time and a name to expected and actual money
    /// </summary>
    [Serializable]
    public class Pondus : Receivable, IMine<IMereo>
    {
        public Pondus(string name)
        {
            My = new Mereo(name);
        }
        public Pondus(string name, Interval interval)
        {
            My = new Mereo(name) {Interval = interval};
        }
        public Pondus(IVoca names)
        {
            My = new Mereo(names);
        }

        public Pondus(IVoca names, Interval interval)
        {
            My = new Mereo(names) {Interval = interval};
        }

        public Pondus(DateTime startDate) : base(startDate)
        {
            My = new Mereo();
        }

        public virtual IMereo My { get; }

        public static Pecuniam GetExpectedSum(IEnumerable<Pondus> items)
        {
            if(items == null || !items.Any())
                return Pecuniam.Zero;

            var p = Pecuniam.Zero;
            foreach (var i in items)
                p += i?.My?.ExpectedValue ?? Pecuniam.Zero;
            return p;
        }

        /// <summary>
        /// Helper method to get an annual sum based on each items interval
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Pecuniam GetExpectedAnnualSum(IEnumerable<Pondus> items)
        {
            if(items == null || !items.Any())
                return Pecuniam.Zero;
            var sum = 0M;
            foreach (var item in items)
            {
                if (item?.My?.ExpectedValue == null ||
                    !Mereo.Interval2AnnualPayMultiplier.ContainsKey(item.My.Interval))
                    continue;
                sum += item.My.ExpectedValue.Amount * Mereo.Interval2AnnualPayMultiplier[item.My.Interval];
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
            var namesEqual = p.My.Equals(My);

            var sDtEq = Inception == p.Inception;
            var eDtEq = Terminus == p.Terminus;

            return namesEqual && sDtEq && eDtEq;
        }

        public override int GetHashCode()
        {
            return Inception.GetHashCode() + Terminus?.GetHashCode() ?? 1 + My?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            var d = new Tuple<string, string, string, string, DateTime?, DateTime?>(My?.ExpectedValue.ToString(), My.Name,
                My.GetName(KindsOfNames.Group), My.Interval.ToString(), Inception, Terminus);
            return d.ToString();
        }
    }
}
