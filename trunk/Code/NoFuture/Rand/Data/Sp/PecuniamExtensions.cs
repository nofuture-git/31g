using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Data.Sp
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
    }
}