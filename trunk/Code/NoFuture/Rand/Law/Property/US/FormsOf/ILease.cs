using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law.Property.US.FormsOf
{
    public interface ILease<T> : ILegalConceptWithProperty<T>, ITempore, IBargain<ILease<T>, T> where T : ILegalProperty
    {
        Predicate<T> IsResidenceHome { get; set; }
    }
}