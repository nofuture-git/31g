using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="IVoca" />
    /// <summary>
    /// A money item which expresses value as a ratio of some time-span.
    /// Is Latin for &apos;earn&apos;.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="IAsset"/>, <see cref="IIdentifier{T}.Value"/> 
    /// has both a get and set.  This type may be denominated by a 
    /// range-of-time but value is not itself a function of-time.
    /// </remarks>
    public interface IMereo : IVoca, IIdentifier<Pecuniam>
    {
        /// <summary>
        /// Readable version of <see cref="TimeDenominator"/>
        /// </summary>
        Interval Interval { get; }

        /// <summary>
        /// Optional, to denote that value is particular to some range-of-time.
        /// </summary>
        TimeSpan? TimeDenominator { get; set; }

        /// <summary>
        /// Other names or common examples of this money entry item 
        /// (e.g. Alimony is also known as Spousal Support)
        /// </summary>
        List<string> GetExempliGratia();

        /// <summary>
        /// Helper function to get Value in some other TimeSpan denomination 
        /// </summary>
        /// <param name="nextFreq"></param>
        /// <returns></returns>
        Pecuniam GetValueInTimespanDenominator(TimeSpan? nextFreq);

        /// <summary>
        /// Same as its overloaded counterpart
        /// </summary>
        /// <param name="totalDays"></param>
        /// <returns></returns>
        Pecuniam GetValueInTimespanDenominator(double totalDays);

    }
}
