using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// when the criminal intent originated with government 
    /// </summary>
    public interface IEntrapment : ILegalConcept
    {
        /// <summary>
        /// this may be subjective or objective depending on the jurisdiction 
        /// </summary>
        Predicate<ILegalPerson> IsIntentOriginFromLawEnforcement { get; set; }
    }
}