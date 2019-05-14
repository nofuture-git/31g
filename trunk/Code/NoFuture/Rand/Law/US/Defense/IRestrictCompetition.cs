using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// contracts that restrict competition
    /// </summary>
    public interface IRestrictCompetition : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[
        /// A covenant not to compete is invalid unless it protects some legitimate 
        /// interest beyond the employer's desire to protect itself from competition.
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsRestraintSelfServing { get; set; }

        /// <summary>
        /// restriction against choice of fiduciaries outweights commercial interest
        /// </summary>
        Predicate<ILegalPerson> IsInjuriousToPublic { get; set; }
    }
}