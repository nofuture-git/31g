using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IIntoxication"/>
    public class Intoxication : DefenseBase, IIntoxication
    {
        public Intoxication() : base(ExtensionMethods.Defendant) { }

        public Intoxication(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public Predicate<ILegalPerson> IsInvoluntary { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsIntoxicated { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;
            var lpTypeName = legalPerson.GetLegalPersonTypeName();
            if (!IsIntoxicated(legalPerson))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(IsIntoxicated)} is false");
                return false;
            }

            if (!IsInvoluntary(legalPerson))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {nameof(IsInvoluntary)} is true");
                return false;
            }

            return true;
        }
    }
}
