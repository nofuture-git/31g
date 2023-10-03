using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Criminal.US.Searches
{
    /// <inheritdoc cref="IMobileVehicleSearch"/>
    public class MobileVehicleSearch : LegalConcept, IMobileVehicleSearch
    {
        public Func<ILegalPerson[], ILegalPerson> GetConductorOfSearch { get; set; }

        public IConsent Consent { get; set; }

        public Predicate<ILegalPerson> IsBeliefEvidenceToCrimeIsPresent { get; set; } = lp => false;

        /// <summary>
        /// The only limit of scope is search areas must derive from size and shape of items being sought
        /// </summary>
        public Predicate<ILegalPerson> IsLimitToAreaToPossibleHidden { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (Consent != null && Consent.IsValid(persons))
            {
                AddReasonEntry($"{nameof(Consent)} {nameof(IsValid)} is true");
                AddReasonEntryRange(Consent.GetReasonEntries());
                return true;
            }

            if (GetConductorOfSearch == null)
                GetConductorOfSearch = lps => null;

            var officer = GetConductorOfSearch(persons);
            if (officer == null)
            {
                AddReasonEntry($"{nameof(GetConductorOfSearch)} returned nothing");
                return false;
            }

            var officerTitle = officer.GetLegalPersonTypeName();

            if (!IsBeliefEvidenceToCrimeIsPresent(officer))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsBeliefEvidenceToCrimeIsPresent)} is false");
                return false;
            }

            if (!IsLimitToAreaToPossibleHidden(officer))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsLimitToAreaToPossibleHidden)} is false");
                return false;
            }

            return true;
        }
    }
}
