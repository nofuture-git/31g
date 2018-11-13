using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp.Enums;

namespace NoFuture.Rand.Sp
{
    /// <inheritdoc cref="IVoca" />
    /// <summary>
    /// Represents a name of any kind of money entry
    /// </summary>
    public interface IMereo : IVoca, IIdentifier<Pecuniam>
    {
        /// <summary>
        /// Readable version of <see cref="DueFrequency"/>
        /// Default is always Annual 
        /// </summary>
        Interval Interval { get; }

        /// <summary>
        /// The numerical equiv. of <see cref="Interval"/> which calling assembly 
        /// must specify.
        /// </summary>
        TimeSpan? DueFrequency { get; set; }

        /// <summary>
        /// Contractual classification of the money item
        /// </summary>
        Classification? Classification { get; set; }

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

        Pecuniam GetValueInTimespanDenominator(double totalDays);

    }
}
