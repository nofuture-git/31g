using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    public class DefenseOfHabitation : DefenseBase
    {
        public DefenseOfHabitation(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// (1) the intruder must have entered or be entering a residence.  Neither outside nor in curtilage counts as entering.
        /// </summary>
        public ObjectivePredicate<ILegalPerson> IsIntruderEnterResidence { get; set; } = lp => false;

        /// <summary>
        /// (2) the residence must be occupied at the time of entrance
        /// </summary>
        public Predicate<ILegalPerson> IsOccupiedResidence { get; set; } = lp => false;

        /// <summary>
        /// (3) objective test that the intruder is a threat to property or person
        /// </summary>
        /// <remarks>
        /// This default to true since is almost always threatening for an intruder to enter a residence
        /// </remarks>
        public ObjectivePredicate<ILegalPerson> IsIntruderThreatening { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = Crime.GetDefendant(persons);
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
