using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// Based on the rules from Arizona v. Gant, 129 S. Ct 1710 (2009)
    /// </summary>
    public class SearchIncidentToArrestMotorVehicle : SearchIncidentBase, IMobileVehicleSearch
    {

        public IConsent Consent { get; set; }

        /// <summary>
        /// the arrestee is unsecured and within reaching distance of passenger compartment at time of search
        /// </summary>
        public Predicate<ILegalPerson> IsArresteeUnsecured { get; set; } = lp => false;

        /// <summary>
        /// Seems to mean that the arrestee could suddenly bolt and reach the passenger compartment 
        /// </summary>
        public Predicate<ILegalPerson> IsArresteeNearPassengerCompartment { get; set; } = lp => false;

        /// <summary>
        /// reasonable to believe that evidence relevant to the crime of arrest
        /// might be found in the vehicle
        /// </summary>
        public Predicate<ILegalPerson> IsBeliefEvidenceToCrimeIsPresent { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (Consent != null && Consent.IsValid(persons))
            {
                AddReasonEntry($"{nameof(Consent)} {nameof(IsValid)} is true");
                AddReasonEntryRange(Consent.GetReasonEntries());
                return true;
            }

            if (!IsValidBeforePredicates(persons))
                return false;
            var suspect = GetSubjectOfSearch(persons);
            var suspectTitle = suspect.GetLegalPersonTypeName();

            var officer = GetConductorOfSearch(persons);
            var officerTitle = officer.GetLegalPersonTypeName();

            //allow implementor to assert these from either perspective 
            var gantItem1 = (IsArresteeUnsecured(suspect) || IsArresteeUnsecured(officer))
                            &&
                            (IsArresteeNearPassengerCompartment(suspect) ||
                             IsArresteeNearPassengerCompartment(officer));

            if (gantItem1)
                return true;

            var gantItem2 = IsBeliefEvidenceToCrimeIsPresent(suspect) ||
                            IsBeliefEvidenceToCrimeIsPresent(officer);

            if (gantItem2)
                return true;

            AddReasonEntry($"{officerTitle} {officer.Name} arrest of {suspectTitle} {suspect.Name}, " +
                           $"{nameof(IsArresteeUnsecured)} and {nameof(IsArresteeNearPassengerCompartment)} " +
                           "are both false");

            AddReasonEntry($"{officerTitle} {officer.Name} arrest of {suspectTitle} {suspect.Name}, " +
                           $"{nameof(IsBeliefEvidenceToCrimeIsPresent)} is false");

            return false;
        }
    }
}
