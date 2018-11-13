using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <summary>
    /// Nf money type extension methods
    /// </summary>
    public static class PecuniamExtensions
    {
        internal static Dictionary<int[], Interval> Days2Interval => new Dictionary<int[], Interval>
        {
            {new[] {1}, Interval.Daily},
            {new[] {7}, Interval.Weekly},
            {new[] {14}, Interval.BiWeekly},
            {new[] {15}, Interval.SemiMonthly},
            {new[] {28,29,30}, Interval.Monthly},
            {new[] {45}, Interval.SemiQuarterly},
            {new[] {90,91}, Interval.Quarterly},
            {new[] {180,182}, Interval.SemiAnnually},
            {new[] {360,365}, Interval.Annually},
        };

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

        public static Pecuniam GetSum(this IEnumerable<Pecuniam> x)
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
    }
}