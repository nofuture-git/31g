using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <inheritdoc cref="IDefenseOfOthers" />
    public class DefenseOfOthers : DefenseOfBase, IDefenseOfOthers
    {
        public DefenseOfOthers() : base(ExtensionMethods.Defendant) { }

        public DefenseOfOthers(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {

        }

        public SubjectivePredicate<ILegalPerson> IsReasonablyAppearedInjuryOrDeath { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;
            if (!base.IsValid(persons))
                return false;

            if (!IsReasonablyAppearedInjuryOrDeath(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsReasonablyAppearedInjuryOrDeath)} is false");
                return false;
            }

            return true;
        }
    }
}
