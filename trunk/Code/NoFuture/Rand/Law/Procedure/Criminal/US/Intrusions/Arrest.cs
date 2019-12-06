using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Procedure.Criminal.US.SearchReasons;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions
{
    /// <summary>
    /// the act of apprehending a person and taking them into custody
    /// to be further questioned or charged with a crime.
    /// </summary>
    public class Arrest : LegalConcept, IIntrusion
    {
        public Func<ILegalPerson[], ILegalPerson> GetSuspect { get; set; } = lps => lps.Suspect();

        public Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; } = lps => lps.LawEnforcement();

        public Predicate<ILegalPerson> IsAwareOfBeingArrested { get; set; } = lp => true;

        public ILegalConcept ProbableCause { get; set; }

        public Predicate<ILegalPerson> IsOccurInPublicPlace { get; set; } = lp => false;

        public IWarrant<ILegalPerson> Warrant { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var suspect = GetSuspect(persons);
            var police = GetLawEnforcement(persons);

            if (suspect == null)
            {
                AddReasonEntry($"{nameof(GetSuspect)} returned nothing");
                return false;
            }

            if (police == null)
            {
                AddReasonEntry($"{nameof(GetLawEnforcement)} returned nothing");
                return false;
            }

            var policeTitle = police.GetLegalPersonTypeName();
            var suspectTitle = suspect.GetLegalPersonTypeName();

            if (ProbableCause == null)
            {
                AddReasonEntry($"{nameof(ProbableCause)} is unassigned");
                return false;
            }

            if (!IsAwareOfBeingArrested(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsAwareOfBeingArrested)} is false");
                return false;
            }

            var isPublicPlace = IsOccurInPublicPlace(police) || IsOccurInPublicPlace(suspect);

            if (isPublicPlace)
                return true;

            var isExigentCircumstances = ProbableCause is ExigentCircumstances;
            if (isExigentCircumstances && ProbableCause.IsValid(persons))
            {
                AddReasonEntry($"{policeTitle} {police.Name}, {nameof(ProbableCause)} " +
                               $"is type {nameof(ExigentCircumstances)}");
                AddReasonEntryRange(ProbableCause.GetReasonEntries());
                return true;
            }

            if (Warrant == null)
            {
                AddReasonEntry($"{policeTitle} {police.Name} arrest of " +
                               $"{suspectTitle} {suspect.Name}, {nameof(IsOccurInPublicPlace)} is false");
                AddReasonEntry($"{nameof(Warrant)} is unassigned");
                return false;
            }

            if (Warrant.ProbableCause == null)
                Warrant.ProbableCause = ProbableCause;

            if (!Warrant.IsValid(persons))
            {
                AddReasonEntry($"{nameof(Warrant)} {nameof(IsValid)} is false");
                AddReasonEntryRange(Warrant.GetReasonEntries());
                return false;
            }

            var lpOnWarrant = Warrant.GetObjectiveOfSearch();

            if (lpOnWarrant == null)
            {
                AddReasonEntry($"{nameof(Warrant)} {nameof(Warrant.GetObjectiveOfSearch)} returned nothing");
                return false;
            }

            var lpOnWarrantTitle = lpOnWarrant.GetLegalPersonTypeName();

            if (!NamesEqual(suspect, lpOnWarrant))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name} to {lpOnWarrantTitle} {lpOnWarrant.Name}, " +
                               $"{nameof(NamesEqual)} is false");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Allows for class level overrides -default is the static VocaBase.Equals
        /// </summary>
        public virtual bool NamesEqual(IVoca voca1, IVoca voca2)
        {
            return VocaBase.Equals(voca1, voca2);
        }

    }
}
