using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Procedure.Criminal.US.SearchReasons;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Challenges
{
    /// <summary>
    /// when the source of evidence is from some other unlawful evidence 
    /// </summary>
    /// <typeparam name="T">The type of the evidence</typeparam>
    [Aka("fruit-of-poisonous-tree")]
    public class DerivativeExclusionaryRule<T> : LegalConcept, IRankable
    {
        public Func<ILegalPerson, T> GetDerivativeEvidence { get; set; } = lp => default(T);
        public Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; } = lps => lps.LawEnforcement();

        /// <summary>
        /// Primary predicate that asserts if the evidence is even in scope of the derivative exclusionary rule
        /// </summary>
        public Predicate<T> IsDerivedFromUnlawfulSource { get; set; } = l => false;

        /// <summary>
        /// Having at least one lawful source will override all unlawful sources
        /// </summary>
        public Predicate<T> IsObtainableFromIndependentSource { get; set; } = l => false;

        /// <summary>
        /// evidence would have been discovered anyway by lawful means
        /// </summary>
        public Predicate<T> IsInevitableDiscovery { get; set; } = l => false;

        /// <summary>
        /// The time period between the illegality and acquisition of secondary
        /// evidence concludes to a dissipation of connection
        /// </summary>
        public Predicate<T> IsLongTimePeriodAttenuated { get; set; } = l => false;

        /// <summary>
        /// The more links in the chain the more attenuated the connection
        /// </summary>
        public Predicate<T> IsInterveningEventsAttenuated { get; set; } = l => false;

        /// <summary>
        /// Deliberate and flagrant the constitutional violation derive to greater attenuation 
        /// </summary>
        public Predicate<ILegalPerson> IsUnintentionalMinorAttenuated { get; set; } = l => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var officer = this.LawEnforcement(persons, GetLawEnforcement);
            if (officer == null)
                return false;

            var officerTitle = officer.GetLegalPersonTypeName();
            var evidence = GetDerivativeEvidence(officer);
            if (object.Equals(evidence, null))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(GetDerivativeEvidence)} returned nothing");
                return false;
            }

            if (IsObtainableFromIndependentSource(evidence))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsObtainableFromIndependentSource)} is true");
                return false;
            }

            if (IsInevitableDiscovery(evidence))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsInevitableDiscovery)} is true");
                return false;
            }

            if (IsLongTimePeriodAttenuated(evidence))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsLongTimePeriodAttenuated)} is true");
                return false;
            }

            if (IsInterveningEventsAttenuated(evidence))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsInterveningEventsAttenuated)} is true");
                return false;
            }

            if (IsUnintentionalMinorAttenuated(officer))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsUnintentionalMinorAttenuated)} is true");
                return false;
            }

            var isDerivative = IsDerivedFromUnlawfulSource(evidence);
            AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsDerivedFromUnlawfulSource)} is {isDerivative}");
            return isDerivative;
        }

        public int GetRank()
        {
            return new ProbableCause().GetRank() + 1;
        }
    }
}
