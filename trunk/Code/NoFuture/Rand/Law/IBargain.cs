using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law
{
    public interface IBargain<T> : ILegalConcept
    {
        [Note("Is the manifestation of willingness to enter into a bargain")]
        ILegalConcept Offer { get; set; }

        Func<ILegalConcept, T> Acceptance { get; set; }

        [Note("expression of approval or agreement")]
        IAssent Assent { get; set; }
    }
}