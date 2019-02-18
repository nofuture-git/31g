using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse
{
    /// <summary>
    /// whenever a person believes their conduct is, in fact, legal.
    /// </summary>
    /// <remarks>
    /// <![CDATA[ (Model Penal Code § 2.04(3) (b)) ]]>
    /// </remarks>
    public class MistakeOfLaw : DefenseBase
    {
        public MistakeOfLaw(ICrime crime) : base(crime)
        {
        }

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

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
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
