using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="Receivable" />
    /// <summary>
    /// A capital concrete composition type to 
    /// bind time and names with expected and actual money value.
    /// Is Latin for weight.
    /// </summary>
    [Serializable]
    public class Pondus : Receivable
    {
        public Pondus(string name)
        {
            Expectation.Name = name;
        }
        public Pondus(string name, Interval interval)
        {
            Expectation.Name = name;
            Expectation.Interval = interval;
        }
        public Pondus(IVoca names)
        {
            Expectation.CopyFrom(names);
        }

        public Pondus(IVoca names, Interval interval)
        {
            Expectation.CopyFrom(names);
            Expectation.Interval = interval;
        }

        public Pondus(DateTime startDate) : base(startDate)
        {
        }

        /// <summary>
        /// Gets the name from the Exceptation
        /// </summary>
        public string Name
        {
            get
            {
                if (Expectation == null)
                    return null;
                var grpName = Expectation.GetName(KindsOfNames.Group);
                return string.IsNullOrWhiteSpace(grpName)
                    ? Expectation.Name
                    : string.Join(", ", grpName, Expectation.Name);
            }
        }

        public static Pecuniam GetExpectedSum(IEnumerable<Pondus> items)
        {
            if(items == null || !items.Any())
                return Pecuniam.Zero;

            var p = Pecuniam.Zero;
            foreach (var i in items)
                p += i?.Expectation?.Value ?? Pecuniam.Zero;
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
                if (item?.Expectation?.Value == null ||
                    !Mereo.Interval2AnnualPayMultiplier.ContainsKey(item.Expectation.Interval))
                    continue;
                sum += item.Expectation.Value.Amount * Mereo.Interval2AnnualPayMultiplier[item.Expectation.Interval];
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
            if (!(obj is Pondus p))
                return base.Equals(obj);
            var namesEqual = p.Expectation.Equals(Expectation);

            var sDtEq = Inception == p.Inception;
            var eDtEq = Terminus == p.Terminus;

            return namesEqual && sDtEq && eDtEq;
        }

        public override int GetHashCode()
        {
            return Inception.GetHashCode() + Terminus?.GetHashCode() ?? 1 + Expectation?.GetHashCode() ?? 1;
        }

        public override string ToString()
        {
            var d = new Tuple<string, string, string, string, DateTime?, DateTime?>(Expectation?.Value.ToString(), Expectation?.Name,
                Expectation?.GetName(KindsOfNames.Group), Expectation?.Interval.ToString(), Inception, Terminus);
            return d.ToString();
        }
    }
}
