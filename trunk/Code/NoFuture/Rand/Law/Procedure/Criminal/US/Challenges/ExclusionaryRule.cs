using System;
using NoFuture.Rand.Law.Procedure.Criminal.US.SearchReasons;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Challenges
{
    /// <summary>
    /// requires the suppression of evidence obtained in violation of the defendant&apos;s constitutional rights
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExclusionaryRule<T> : LegalConcept, IRankable
    {
        private DerivativeExclusionaryRule<T> _derivativeExclusionary;

        public Func<ILegalPerson, T> GetEvidence { get; set; } = lp => default(T);
        public Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; } = lps => lps.LawEnforcement();

        public Predicate<T> IsObtainedThroughUnlawfulMeans { get; set; } = l => false;

        public DerivativeExclusionaryRule<T> DerivativeExclusion
        {
            get => _derivativeExclusionary;
            set
            {
                _derivativeExclusionary = value;
                if (_derivativeExclusionary == null) 
                    return;
                _derivativeExclusionary.GetDerivativeEvidence = GetEvidence;
                _derivativeExclusionary.GetLawEnforcement = GetLawEnforcement;
            }
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var officer = GetLawEnforcement(persons);
            if (officer == null)
            {
                AddReasonEntry($"{nameof(GetLawEnforcement)} returned nothing");
                return false;
            }

            var officerTitle = officer.GetLegalPersonTypeName();
            var evidence = GetEvidence(officer);
            if (object.Equals(evidence, null))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(GetEvidence)} returned nothing");
                return false;
            }

            var isUnlawful = IsObtainedThroughUnlawfulMeans(evidence);
            AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsObtainedThroughUnlawfulMeans)} is {isUnlawful}");

            if (isUnlawful)
            {
                return true;
            }

            if (DerivativeExclusion == null)
                return false;

            isUnlawful = DerivativeExclusion.IsValid(persons);
            AddReasonEntryRange(DerivativeExclusion.GetReasonEntries());
            return isUnlawful;
        }

        public int GetRank()
        {
            return new ProbableCause().GetRank() + 1;
        }
    }
}
