using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Nf money type extension methods
    /// </summary>
    public static class PecuniamExtensions
    {
        private static Dictionary<Interval, int> _interval2Multiplier;
        public static Dictionary<int[], Interval> Days2Interval => new Dictionary<int[], Interval>
        {
            {new[] {1}, Interval.Daily},
            {new[] {7}, Interval.Weekly},
            {new[] {14}, Interval.BiWeekly},
            {new[] {15}, Interval.SemiMonthly},
            {new[] {30,28,29}, Interval.Monthly},
            {new[] {45}, Interval.SemiQuarterly},
            {new[] {90,91}, Interval.Quarterly},
            {new[] {180,182}, Interval.SemiAnnually},
            {new[] {360,365}, Interval.Annually},
        };

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

        public static Pecuniam ToPecuniam(this double x)
        {
            return new Pecuniam((decimal)Math.Round(x,2));
        }
        public static Pecuniam ToPecuniam(this int x)
        {
            return new Pecuniam(x);
        }

        public static Pecuniam ToPecuniam(this decimal x)
        {
            return new Pecuniam(x);
        }

        public static Pecuniam Sum(this IEnumerable<Pecuniam> x)
        {
            return x == null ? Pecuniam.Zero : x.Aggregate(Pecuniam.Zero, (current, i) => current + i);
        }

        /// <summary>
        /// Helper method to convert a Nf Interval into .NET <see cref="TimeSpan"/>
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        public static TimeSpan? ToTimeSpan(this Interval df)
        {
            foreach (var d2i in Days2Interval)
            {
                if (d2i.Value == df)
                {
                    var days = d2i.Key.First();
                    return new TimeSpan(days, 0,0,0);
                }
            }

            return null;
        }

        /// <summary>
        /// Helper method to convert a .NET <see cref="TimeSpan"/>
        /// into a Nf Interval
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        public static Interval? ToInterval(this TimeSpan? df)
        {
            if (df == null)
                return null;
            var days = df.Value.Days;

            foreach (var d2i in Days2Interval)
            {
                if (d2i.Key.Contains(days))
                    return d2i.Value;
            }

            return null;
        }

        /// <summary>
        /// Divides <see cref="Constants.TropicalYear"/> tick by 12
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetTropicalMonth()
        {
            var tropicalYearTicks = Constants.TropicalYear.Ticks;
            return new TimeSpan(tropicalYearTicks/12L);
        }

        /// <summary>
        /// Calculates the sum of all item&apos;s Value
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Pecuniam Sum(this IEnumerable<IReceivable> items)
        {
            if (items == null || !items.Any())
                return Pecuniam.Zero;

            var p = Pecuniam.Zero;
            foreach (var i in items)
                p += i?.Value ?? Pecuniam.Zero;
            return p;
        }

        /// <summary>
        /// Gets the sum of the average-per-year for each of the items
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Pecuniam AnnualSum(this IEnumerable<IReceivable> items)
        {
            if (items == null || !items.Any())
                return Pecuniam.Zero;

            var p = Pecuniam.Zero;
            foreach (var i in items)
                p += i?.AveragePerDueFrequency(Constants.TropicalYear) ?? Pecuniam.Zero;
            return p;
        }

        /// <summary>
        /// Gets the sum of each transaction&apos;s Cash
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Pecuniam Sum(this IEnumerable<ITransaction> items)
        {
            if(items == null || !items.Any())
                return Pecuniam.Zero;

            return items.Select(t => t.Cash).Sum();
        }

        /// <summary>
        /// Calculates the sum of all item&apos;s Value
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Pecuniam Sum<T>(this IEnumerable<IAccount<T>> items)
        {
            if (items == null || !items.Any())
                return Pecuniam.Zero;

            var p = Pecuniam.Zero;
            foreach (var i in items)
                p += i?.Value ?? Pecuniam.Zero;
            return p;
        }
    }
}