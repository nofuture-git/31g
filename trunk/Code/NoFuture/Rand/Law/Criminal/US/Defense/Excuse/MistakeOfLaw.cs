using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IMistakeOfLaw"/>
    public class MistakeOfLaw : DefenseBase, IMistakeOfLaw
    {
        public MistakeOfLaw() : base(ExtensionMethods.Defendant) { }

        public MistakeOfLaw(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public Predicate<ILegalPerson> IsRelianceOnStatementOfLaw { get; set; } = lp => false;

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
