using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IAgeOfMajority"/>
    public class Infancy: DefenseBase, IAgeOfMajority
    {
        public Infancy() : base(ExtensionMethods.Defendant) { }

        public Infancy(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public Predicate<ILegalPerson> IsUnderage { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;

            var isAdult = !IsUnderage(legalPerson);

            if (isAdult)
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsUnderage)} is false");
                return false;
            }

            return true;
        }
    }
}
