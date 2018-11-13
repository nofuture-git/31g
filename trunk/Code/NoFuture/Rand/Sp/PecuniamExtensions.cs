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

        public static TimeSpan? ConvertInterval(this Interval df)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper method to convert a .NET <see cref="TimeSpan"/>
        /// into a Nf Interval
        /// </summary>
        /// <param name="df"></param>
        /// <returns></returns>
        public static Interval? ConvertTimespan(this TimeSpan? df)
        {
            if (df == null)
                return null;
            var days = df.Value.Days;

            switch (days)
            {
                case 0:
                    return null;
                case 1:
                    return Interval.Daily;
                case 7:
                    return Interval.Weekly;
                case 14:
                    return Interval.BiWeekly;
                case 15:
                    return Interval.SemiMonthly;
                case 28:
                case 29:
                case 30:
                    return Interval.Monthly;
                case 45:
                    return Interval.SemiQuarterly;
                case 90:
                case 91:
                    return Interval.Quarterly;
                case 180:
                case 182:
                    return Interval.SemiAnnually;
                case 360:
                case 365:
                    return Interval.Annually;
                default:
                    return null;

            }
        }
    }
}