using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense
{
    /// <inheritdoc cref="IPolicePower"/>
    public class PolicePower : DefenseBase, IPolicePower
    {
        public PolicePower() : base(ExtensionMethods.Defendant) { }

        public PolicePower(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public Predicate<ILegalPerson> IsAgentOfTheState { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsReasonableUseOfForce { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;
            var lpPersonType = legalPerson.GetLegalPersonTypeName();
            if (!IsAgentOfTheState(legalPerson))
            {
                AddReasonEntry($"{lpPersonType}, {legalPerson.Name}, {nameof(IsAgentOfTheState)} is false");
                return false;
            }

            if (!IsReasonableUseOfForce(legalPerson))
            {
                AddReasonEntry($"{lpPersonType}, {legalPerson.Name}, {nameof(IsReasonableUseOfForce)} is false");
                return false;
            }

            return true;
        }
    }
}
