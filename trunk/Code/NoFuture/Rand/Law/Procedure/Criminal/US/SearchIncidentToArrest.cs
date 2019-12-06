using System;
using NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// After being placed under arrest a search of the person
    /// and the area within their reach may be searched without any warrant
    /// </summary>
    public class SearchIncidentToArrest : ExpectationOfPrivacy
    {
        public Arrest Arrest { get; set; }

        /// <summary>
        /// Search of the arrested person&apos;s clothes, pockets and pocket items
        /// </summary>
        public Predicate<ILegalPerson> IsSearchArrestedPerson { get; set; } = lp => false;

        /// <summary>
        /// Search of the arrested person&apos;s &quot;grabable space&quot; 
        /// </summary>
        public Predicate<ILegalPerson> IsSearchImmediateArea { get; set; } = lp => false;

        /// <summary>
        /// reasonable to believe that evidence relevant to the crime of arrest
        /// might be found in the vehicle
        /// </summary>
        public Predicate<ILegalPerson> IsSearchOfMotorVehicle { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            
            if (Arrest == null)
            {
                AddReasonEntry($"{nameof(Arrest)} is unassigned");
                return false;
            }

            if (Arrest.GetSuspect == null)
                Arrest.GetSuspect = GetSubjectOfSearch;

            if (Arrest.GetLawEnforcement == null)
                Arrest.GetLawEnforcement = GetConductorOfSearch;

            if (!Arrest.IsValid(persons))
            {
                AddReasonEntry($"{nameof(Arrest)} {nameof(IsValid)} is false");
                AddReasonEntryRange(Arrest.GetReasonEntries());
                return false;
            }

            var subjectPerson = GetSubjectOfSearch(persons);

            if (subjectPerson == null)
            {
                AddReasonEntry($"{nameof(GetSubjectOfSearch)} returned nothing");
                return false;
            }

            var title = subjectPerson.GetLegalPersonTypeName();

            if (IsSearchArrestedPerson(subjectPerson) 
                || IsSearchImmediateArea(subjectPerson)
                || IsSearchOfMotorVehicle(subjectPerson))
                return true;

            AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsSearchArrestedPerson)} " +
                           $", {nameof(IsSearchImmediateArea)} and " +
                           $"{nameof(IsSearchOfMotorVehicle)} are all false");

            return false;
        }
    }
}
