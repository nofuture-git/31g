using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// After being placed under arrest a search of the person
    /// and the area within their reach may be searched without any warrant
    /// </summary>
    public class SearchIncidentToArrest : SearchIncidentBase
    {
        /// <summary>
        /// Search of the arrested person&apos;s clothes, pockets and pocket items
        /// </summary>
        public Predicate<ILegalPerson> IsSearchArrestedPerson { get; set; } = lp => false;

        /// <summary>
        /// Search of the arrested person&apos;s &quot;grabable space&quot; 
        /// </summary>
        public Predicate<ILegalPerson> IsSearchImmediateArea { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsValidBeforePredicates(persons))
                return false;
            var suspect = GetSubjectOfSearch(persons);
            var suspectTitle = suspect.GetLegalPersonTypeName();

            var officer = GetConductorOfSearch(persons);
            var officerTitle = officer.GetLegalPersonTypeName();

            if (IsSearchArrestedPerson(suspect) 
                || IsSearchImmediateArea(suspect)
                || IsSearchArrestedPerson(officer)
                || IsSearchImmediateArea(officer))
                return true;

            AddReasonEntry($"{officerTitle} {officer.Name} arrest of {suspectTitle} {suspect.Name}, " +
                           $"{nameof(IsSearchArrestedPerson)}  and {nameof(IsSearchImmediateArea)} are both false");

            return false;
        }
    }
}
