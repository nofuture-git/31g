using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Contract.US
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

        /// <summary>
        /// An outward expression of approval that a reasonable person would understand
        /// </summary>
        /// <remarks>
        /// src [LUCY v. ZEHMER Supreme Court of Virginia 196 Va. 493; 84 S.E.2d 516 (1954)]
        /// <![CDATA[
        /// If his words and acts, judged by a reasonable standard, manifest an intention 
        /// to agree, it is immaterial what may be the real but unexpressed state of his mind.
        /// ]]>
        /// </remarks>
        [Note("expression of approval or agreement")]
        IContractTerms Assent { get; set; }

        ILegalPerson GetOfferor(ILegalPerson[] persons);

        ILegalPerson GetOfferee(ILegalPerson[] persons);
    }
}
