using System;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <inheritdoc cref="IDefenseOfHabitation"/>
    public class DefenseOfHabitation : DefenseBase, IDefenseOfHabitation
    {
        public DefenseOfHabitation() : base(ExtensionMethods.Defendant) { }

        public DefenseOfHabitation(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public ObjectivePredicate<ILegalPerson> IsIntruderEnterResidence { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsOccupiedResidence { get; set; } = lp => false;

        public ObjectivePredicate<ILegalPerson> IsIntruderThreatening { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;
            var lpTypeName = legalPerson.GetLegalPersonTypeName();
            if (persons.All(lp => !IsIntruderEnterResidence(lp)))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {IsIntruderEnterResidence} is false " +
                               $"for {lpTypeName} and all other parties");
                return false;
            }

            if (persons.All(lp => !IsOccupiedResidence(lp)))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {IsOccupiedResidence} is false " +
                               $"for {lpTypeName} and all other parties");
                return false;
            }
            if (persons.All(lp => !IsIntruderThreatening(lp)))
            {
                AddReasonEntry($"{lpTypeName}, {legalPerson.Name}, {IsIntruderThreatening} is false " +
                               $"for {lpTypeName} and all other parties");
                return false;
            }

            return true;
        }
    }
}
