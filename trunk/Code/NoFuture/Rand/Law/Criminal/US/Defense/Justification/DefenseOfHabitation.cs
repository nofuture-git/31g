using System;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <inheritdoc cref="IDefenseOfHabitation"/>
    public class DefenseOfHabitation : DefenseBase, IDefenseOfHabitation
    {
        public ObjectivePredicate<ILegalPerson> IsIntruderEnterResidence { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsOccupiedResidence { get; set; } = lp => false;

        public ObjectivePredicate<ILegalPerson> IsIntruderThreatening { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (persons.All(lp => !IsIntruderEnterResidence(lp)))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {IsIntruderEnterResidence} is false " +
                               "for defendant and all other parties");
                return false;
            }

            if (persons.All(lp => !IsOccupiedResidence(lp)))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {IsOccupiedResidence} is false " +
                               "for defendant and all other parties");
                return false;
            }
            if (persons.All(lp => !IsIntruderThreatening(lp)))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {IsIntruderThreatening} is false " +
                               "for defendant and all other parties");
                return false;
            }

            return true;
        }
    }
}
