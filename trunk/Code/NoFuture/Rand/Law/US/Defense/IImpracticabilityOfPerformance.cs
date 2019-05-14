using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// <![CDATA[ 
    /// Restatement (Second) of Contracts § 261 
    /// "Impracticability" means more than "impracticality"
    /// ]]>
    /// </summary>
    public interface IImpracticabilityOfPerformance : ILegalConcept
    {
        /// <summary>
        /// occurrence of an event the non-occurrence of which was a basic assumption on which the contract was made.
        /// </summary>
        /// <remarks>
        /// economic circumstances would need to be economic shocks or the like.
        /// </remarks>
        Predicate<ILegalPerson> IsBasicAssumptionGone { get; set; }

        /// <summary>
        /// <![CDATA[
        /// a party's performance is made impracticable without his fault
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsAtFault { get; set; }

        /// <summary>
        /// unless the language or the circumstances indicate the contrary
        /// </summary>
        Predicate<ILegalPerson> IsContraryInForm { get; set; }
    }
}