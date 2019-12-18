using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    /// <summary>
    /// A contract to lease some property
    /// </summary>
    /// <typeparam name="T">The kind of property being leased</typeparam>
    /// <inheritdoc cref="IBargain{T,M}"/>
    public interface ILease<T> : ILegalConceptWithProperty<T>, ITempore, IAssentTerms, IBargain<ILease<T>, T> where T : ILegalProperty
    {
        /// <summary>
        /// More protection is given to tenants for a lease on their home residence
        /// </summary>
        Predicate<T> IsResidenceHome { get; set; }

        /// <summary>
        /// The current time on which <see cref="IsLeaseExpired"/> is asserted, default to now.
        /// </summary>
        DateTime CurrentDateTime { get; set; }

        /// <summary>
        /// Eval <see cref="CurrentDateTime"/> <see cref="ITempore.IsInRange"/>
        /// </summary>
        bool IsLeaseExpired { get; }
    }
}