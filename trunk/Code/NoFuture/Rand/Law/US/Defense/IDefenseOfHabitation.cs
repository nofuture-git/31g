using System;

namespace NoFuture.Rand.Law.US.Defense
{
    public interface IDefenseOfHabitation : ILegalConcept
    {
        /// <summary>
        /// (1) the intruder must have entered or be entering a residence.  Neither outside nor in curtilage counts as entering.
        /// </summary>
        ObjectivePredicate<ILegalPerson> IsIntruderEnterResidence { get; set; }

        /// <summary>
        /// (2) the residence must be occupied at the time of entrance
        /// </summary>
        Predicate<ILegalPerson> IsOccupiedResidence { get; set; }

        /// <summary>
        /// (3) objective test that the intruder is a threat to property or person
        /// </summary>
        /// <remarks>
        /// This default to true since is almost always threatening for an intruder to enter a residence
        /// </remarks>
        ObjectivePredicate<ILegalPerson> IsIntruderThreatening { get; set; }

    }
}