using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp.Enums;

namespace NoFuture.Rand.Data.Sp
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a name of any kind of money entry
    /// </summary>
    public interface IMereo : IVoca
    {
        /// <summary>
        /// The time frame associated to this money entry
        /// </summary>
        Interval Interval { get; set; }

        Classification Classification { get; set; }

        string Name { get; set; }

        /// <summary>
        /// Other names or common examples of this money entry item 
        /// (e.g. Alimony is also known as Spousal Support)
        /// </summary>
        List<string> ExempliGratia { get; }

        /// <summary>
        /// The expected money worth
        /// </summary>
        Pecuniam ExpectedValue { get; set; }

        /// <summary>
        /// Both assigns <see cref="Interval"/> to Annually and increases the
        /// <see cref="ExpectedValue"/> to match.
        /// </summary>
        void AdjustToAnnualInterval();
    }
}
