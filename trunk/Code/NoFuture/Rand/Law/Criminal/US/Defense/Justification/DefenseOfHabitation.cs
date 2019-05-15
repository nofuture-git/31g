using System;
using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    public class DefenseOfHabitation : DefenseBase
    {
        public DefenseOfHabitation() : base(ExtensionMethods.Defendant) { }

        public DefenseOfHabitation(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
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
