using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// the facts as the defendant believes them to be negate the requisite intent for the crime at issue
    /// </summary>
    public class MistakeOfFact : DefenseBase
    {
        public MistakeOfFact() : base(ExtensionMethods.Defendant) { }

        public MistakeOfFact(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public SubjectivePredicate<ILegalPerson> IsBeliefNegateIntent { get; set; } = lp => false;

        public bool IsStrictLiability { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = this.Defendant(persons);
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
