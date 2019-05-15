using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IMistakeOfFact"/>
    public class MistakeOfFact : DefenseBase, IMistakeOfFact
    {
        public MistakeOfFact() : base(ExtensionMethods.Defendant) { }

        public MistakeOfFact(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public SubjectivePredicate<ILegalPerson> IsBeliefNegateIntent { get; set; } = lp => false;

        public bool IsStrictLiability { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;
            var lpTypeName = legalPerson.GetLegalPersonTypeName();
            if (IsStrictLiability)
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(IsStrictLiability)} is true");
                return false;
            }

            if (!IsBeliefNegateIntent(legalPerson))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(IsBeliefNegateIntent)} is false");
                return false;
            }

            return true;
        }
    }
}
