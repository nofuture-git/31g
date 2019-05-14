using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// an affirmative defense for Conspiracy
    /// </summary>
    public interface IRenunciation : ILegalConcept
    {
        Predicate<ILegalPerson> IsVoluntarily { get; set; }

        Predicate<ILegalPerson> IsCompletely { get; set; }

        /// <summary>
        /// The conspiracy is a plan to commit some crime which 
        /// is called the object of the conspiracy.  It is this 
        /// crime that must have been thwarted.
        /// </summary>
        Predicate<ILegalPerson> IsResultCrimeThwarted { get; set; }
    }
}