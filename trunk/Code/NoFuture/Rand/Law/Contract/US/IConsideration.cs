using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Contract.US
{
    /// <summary>
    /// A contract common law concept.  A contract always has an Offeror and an
    /// Offeree.  This is the requirement that, after the exchange, both Offeror
    /// and Offeree got what they wanted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Aka("quid pro quo", "this for that")]
    public interface IConsideration<T> : ILegalConcept
    {
        Func<ILegalPerson, T, bool> IsSoughtByOfferor { get; set; }
        Func<ILegalPerson, ILegalConcept, bool> IsGivenByOfferee { get; set; }
        Predicate<ILegalConcept> IsValueInEyesOfLaw { get; set; }
    }
}
