using System;
using NoFuture.Law.Attributes;

namespace NoFuture.Law.Contract.US
{
    /// <summary>
    /// A contract common law concept.  A contract always has an Offeror and an
    /// Offeree.  This is the requirement that, after the exchange, both Offeror
    /// and Offeree got what they wanted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Aka("quid pro quo", "this for that", "mutuality of obligation")]
    [Note("is performance or return promise bargained for")]
    public interface IConsideration<T> : ILegalConcept
    {
        /// <summary>
        /// A test for if Acceptance is actually what the offeror wants in return.
        /// </summary>
        Func<ILegalPerson, T, bool> IsSoughtByOfferor { get; set; }

        /// <summary>
        /// A test for if Offer is actually what the offeree wants in return.
        /// </summary>
        Func<ILegalPerson, ILegalConcept, bool> IsGivenByOfferee { get; set; }

        /// <summary>
        /// What is bargained for must have value
        /// </summary>
        Predicate<ILegalConcept> IsValueInEyesOfLaw { get; set; }

        /// <summary>
        /// What is bargained for must not be a choice - it must be a duty
        /// </summary>
        [Note("is not just a choice")]
        Predicate<ILegalConcept> IsIllusoryPromise { get; set; }

        /// <summary>
        /// What is bargained for must not be an existing duty.  When it 
        /// is an existing duty there is no consideration and whatever the other side
        /// is bringing is a gift.
        /// </summary>
        /// <remarks>
        /// <![CDATA[ e.g. cannot bargain that we promise not to steal from each other]]>
        /// </remarks>
        [Note("is not already obligated to be done")]
        Predicate<ILegalConcept> IsExistingDuty { get; set; }
    }
}
