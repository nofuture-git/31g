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
        /// The time frame associated to this money entry
        /// </summary>
        Interval Interval { get; set; }

        /// <summary>
        /// Contractual classification of the money item
        /// </summary>
        Classification Classification { get; set; }

        /// <summary>
        /// Other names or common examples of this money entry item 
        /// (e.g. Alimony is also known as Spousal Support)
        /// </summary>
        List<string> GetExempliGratia();

        /// <summary>
        /// Both assigns <see cref="Interval"/> to Annually and increases the
        /// Value to match.
        /// </summary>
        void AdjustToAnnualInterval();
    }
}
