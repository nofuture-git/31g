using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <inheritdoc />
    /// <remarks>
    /// <![CDATA[
    /// It is axiomatic in modern contracts law that all contracts 
    /// are, in some fashion, "incomplete."  Meaning, no contract 
    /// has addressed every possible contingency.
    /// ]]>
    /// </remarks>
    [Note("Latin assimilation of 'com' (with, together) + 'trahere' (to pull, drag)")]
    public interface IContract<T> : ILegalConcept
    {
        [Note("Is the manifestation of willingness to enter into a bargain")]
        ILegalConcept Offer { get; set; }

        Func<ILegalConcept, T> Acceptance { get; set; }

        [Note("expression of approval or agreement")]
        IAssent Assent { get; set; }

        ILegalPerson GetOfferor(ILegalPerson[] persons);

        ILegalPerson GetOfferee(ILegalPerson[] persons);
    }
}
