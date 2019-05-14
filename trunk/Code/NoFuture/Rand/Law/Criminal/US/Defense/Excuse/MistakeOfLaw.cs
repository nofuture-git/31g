using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IMistakeOfLaw"/>
    public class MistakeOfLaw : DefenseBase, IMistakeOfLaw
    {
        public Predicate<ILegalPerson> IsRelianceOnStatementOfLaw { get; set; } = lp => false;

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
