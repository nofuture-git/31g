using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// whenever a person believes their conduct is, in fact, legal.
    /// </summary>
    /// <remarks>
    /// <![CDATA[ (Model Penal Code § 2.04(3) (b)) ]]>
    /// </remarks>
    public interface IMistakeOfLaw : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[
        /// conduct when…the actor...acts in reasonable reliance upon an official statement of the law
        /// ]]>
        /// </summary>
        /// <remarks>
        /// reliance on the statement of an attorney-at-law is not sufficient
        /// </remarks>
        Predicate<ILegalPerson> IsRelianceOnStatementOfLaw { get; set; }

        /// <summary>
        /// <![CDATA[
        /// afterward determined to be invalid...contained in...a statute or...judicial decision
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsStatementOfLawNowInvalid { get; set; }
    }
}