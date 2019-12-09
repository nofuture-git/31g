using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Procedure.Criminal.US.SearchReasons;
using NoFuture.Rand.Law.Procedure.Criminal.US.Warrants;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Challenges
{
    /// <summary>
    /// requires the suppression of evidence obtained in violation of the defendant&apos;s constitutional rights
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExclusionaryRule<T> : LegalConcept, IRankable
    {
        public Func<ILegalPerson[], ILegalPerson> GetSuspect { get; set; } = lps => lps.Suspect();
        public Func<ILegalPerson, T> GetEvidence { get; set; } = lp => default(T);
        public Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; } = lps => lps.LawEnforcement();

        public Predicate<T> IsObtainedThroughUnlawfulMeans { get; set; } = l => false;

        /// <summary>
        /// only those who are actual victims of the alleged violation have standing to challenge
        /// </summary>
        [Aka("standing")]
        public Predicate<ILegalPerson> IsDirectlyEffectedByViolation { get; set; } = lp => false;

        /// <summary>
        /// the exclusionary rule can only be applied to the criminal trial,
        /// not civil proceedings, grand jury nor habeas corpus proceedings
        /// </summary>
        public Predicate<T> IsUseInCriminalTrial { get; set; } = l => false;

        /// <summary>
        /// when applicable, the search warrant, on which the evidence was seized, must be valid
        /// </summary>
        public SearchWarrant SearchWarrant { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var officer = GetLawEnforcement(persons);
            var suspect = GetSuspect(persons);
            if (officer == null)
            {
                AddReasonEntry($"{nameof(GetLawEnforcement)} returned nothing");
                return false;
            }

            if (suspect == null)
            {
                AddReasonEntry($"{nameof(GetSuspect)} returned nothing");
                return false;
            }

            var officerTitle = officer.GetLegalPersonTypeName();
            var suspectTitle = suspect.GetLegalPersonTypeName();

            var evidence = GetEvidence(officer);
            if (object.Equals(evidence, null))
            {
                evidence = GetEvidence(suspect);
            }

            if (object.Equals(evidence, null))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(GetEvidence)} returned nothing");
                return false;
            }

            var isUnlawful = IsObtainedThroughUnlawfulMeans(evidence);
            if (!IsObtainedThroughUnlawfulMeans(evidence))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsObtainedThroughUnlawfulMeans)} is false");
                return false;
            }

            if (!IsDirectlyEffectedByViolation(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsDirectlyEffectedByViolation)} is false");
                return false;
            }

            if (!IsUseInCriminalTrial(evidence))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsUseInCriminalTrial)} is false");
                return false;
            }

            if (SearchWarrant != null && !SearchWarrant.IsValid(persons))
            {
                AddReasonEntryRange(SearchWarrant.GetReasonEntries());
                return false;
            }
            

            return isUnlawful;
        }

        public int GetRank()
        {
            return new ProbableCause().GetRank() + 1;
        }
    }
}
