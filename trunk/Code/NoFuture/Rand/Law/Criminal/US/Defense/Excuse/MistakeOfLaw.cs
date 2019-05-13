using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// whenever a person believes their conduct is, in fact, legal.
    /// </summary>
    /// <remarks>
    /// <![CDATA[ (Model Penal Code § 2.04(3) (b)) ]]>
    /// </remarks>
    public class MistakeOfLaw : DefenseBase
    {
        /// <summary>
        /// <![CDATA[
        /// conduct when…the actor...acts in reasonable reliance upon an official statement of the law
        /// ]]>
        /// </summary>
        /// <remarks>
        /// reliance on the statement of an attorney-at-law is not sufficient
        /// </remarks>
        public Predicate<ILegalPerson> IsRelianceOnStatementOfLaw { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[
        /// afterward determined to be invalid…contained in...a statute or...judicial decision
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsStatementOfLawNowInvalid { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsRelianceOnStatementOfLaw(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsRelianceOnStatementOfLaw)} is false");
                return false;
            }

            if (!IsStatementOfLawNowInvalid(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsStatementOfLawNowInvalid)} is false");
                return false;
            }

            return true;
        }
    }
}
