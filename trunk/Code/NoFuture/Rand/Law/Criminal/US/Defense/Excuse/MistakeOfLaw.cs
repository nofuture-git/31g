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
        public MistakeOfLaw() : base(ExtensionMethods.Defendant) { }

        public MistakeOfLaw(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

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
        /// afterward determined to be invalid...contained in...a statute or...judicial decision
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsStatementOfLawNowInvalid { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;
            var lpTypeName = legalPerson.GetLegalPersonTypeName();
            if (!IsRelianceOnStatementOfLaw(legalPerson))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(IsRelianceOnStatementOfLaw)} is false");
                return false;
            }

            if (!IsStatementOfLawNowInvalid(legalPerson))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(IsStatementOfLawNowInvalid)} is false");
                return false;
            }

            return true;
        }
    }
}
