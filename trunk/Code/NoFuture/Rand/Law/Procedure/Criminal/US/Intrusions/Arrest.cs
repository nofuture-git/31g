using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions
{
    /// <summary>
    /// the act of apprehending a person and taking them into custody
    /// to be further questioned or charged with a crime.
    /// </summary>
    public class Arrest : ImplicatingFourthAmendment<ILegalPerson>
    {
        public Predicate<ILegalPerson> IsAwareOfBeingArrested { get; set; } = lp => true;

        public Predicate<ILegalPerson> IsOccurInPublicPlace { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var suspect = GetSuspect(persons);
            var police = GetLawEnforcement(persons);

            if (suspect == null)
            {
                AddReasonEntry($"{nameof(GetSuspect)} returned nothing");
                return false;
            }
            var suspectTitle = suspect.GetLegalPersonTypeName();

            if (!IsAwareOfBeingArrested(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, " +
                               $"{nameof(IsAwareOfBeingArrested)} is false");
                return false;
            }
            var isWarrantValid = Warrant != null && IsWarrantValid(persons);

            if (isWarrantValid)
                return true;

            if (police == null)
            {
                AddReasonEntry($"{nameof(GetLawEnforcement)} returned nothing");
                return false;
            }

            var policeTitle = police.GetLegalPersonTypeName();

            if (!IsProbableCauseExigentCircumstances(persons))
                return false;

            var isPublicPlace = IsOccurInPublicPlace(police) || IsOccurInPublicPlace(suspect);

            if (isPublicPlace)
                return true;

            AddReasonEntry($"{policeTitle} {police.Name} arrest of {suspectTitle} {suspect.Name}, " +
                           $"{nameof(IsOccurInPublicPlace)} is false");
            return false;
        }


    }
}
