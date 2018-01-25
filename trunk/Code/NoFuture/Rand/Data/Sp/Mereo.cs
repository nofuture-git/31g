using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <inheritdoc cref="IMereo"/>
    /// <inheritdoc cref="VocaBase"/>
    /// <summary>
    /// Base implementation a name of any kind of money entry
    /// </summary>
    [Serializable]
    public class Mereo : VocaBase, IMereo
    {
        private static Dictionary<Interval, int> _interval2Multiplier;
        private readonly List<string> _eg = new List<string>();
        private Pecuniam _expectedValue = Pecuniam.Zero;
        public Mereo()
        {
        }

        public Mereo(string name):base(name)
        {
        }

        public Mereo(string name, string groupName):base(name, groupName)
        {
        }

        public Mereo(IVoca names)
        {
            CopyFrom(names);
        }

        public Mereo(IMereo mereo) : this((IVoca)mereo)
        {
            ExpectedValue = mereo.ExpectedValue;
            Interval = mereo.Interval;
            Classification = mereo.Classification;
            foreach(var eg in mereo.ExempliGratia)
                ExempliGratia.Add(eg);
        }

        public Interval Interval { get; set; }
        public Classification Classification { get; set; }

        public virtual string Name
        {
            get => GetName(KindsOfNames.Legal);
            set => UpsertName(KindsOfNames.Legal, value);
        }

        public List<string> ExempliGratia => _eg;

        public Pecuniam ExpectedValue
        {
            get => _expectedValue ?? (_expectedValue = Pecuniam.Zero);
            set => _expectedValue = value;
        }

        public void AdjustToAnnualInterval()
        {
            if (Interval == Interval.Annually)
                return;

            var hasExpectedValue = ExpectedValue != null && ExpectedValue != Pecuniam.Zero;
            var hasMultiplier = Interval2AnnualPayMultiplier.ContainsKey(Interval);

            if (!hasExpectedValue || !hasMultiplier)
            {
                Interval = Interval.Annually;
                return;
            }

            var multiplier = Interval2AnnualPayMultiplier[Interval];
            ExpectedValue = (ExpectedValue.ToDouble() * multiplier).ToPecuniam();
            Interval = Interval.Annually;
        }

        public override string ToString()
        {
            return new Tuple<string, string, Pecuniam, Interval>(Name, GetName(KindsOfNames.Group), ExpectedValue,
                Interval).ToString();
        }
        /// <summary>
        /// A general table to align an interval to some annual multiplier
        /// (e.g. Hourly means 52 weeks * 40 hours per week = 2080)
        /// </summary>
        public static Dictionary<Interval, int> Interval2AnnualPayMultiplier
        {
            get
            {
                if (_interval2Multiplier != null)
                    return _interval2Multiplier;

                _interval2Multiplier = new Dictionary<Interval, int>
                {
                    {Interval.OnceOnly, 1},
                    {Interval.Hourly, 2080},
                    {Interval.Daily, 260},
                    {Interval.Weekly, 52},
                    {Interval.BiWeekly, 26},
                    {Interval.SemiMonthly, 24},
                    {Interval.Monthly, 12},
                    {Interval.Quarterly, 4},
                    {Interval.SemiAnnually, 2},
                    {Interval.Annually, 1},
                };

                return _interval2Multiplier;
            }
        }

    }
}
