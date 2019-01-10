using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    public interface IContract<T>
    {
        [Note("Is the manifestation of willingness to enter into a bargain")]
        IObjectiveLegalConcept Offer { get; set; }

        Func<IObjectiveLegalConcept, T> Acceptance { get; set; }
    }
}
