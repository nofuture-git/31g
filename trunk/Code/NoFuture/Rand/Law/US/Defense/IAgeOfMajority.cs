using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// Defendant is not subject to criminal prosecution because, being so young, they cannot form criminal intent
    /// </summary>
    public interface IAgeOfMajority : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[
        /// contracts of minors are voidable
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsUnderage { get; set; }
    }
}
