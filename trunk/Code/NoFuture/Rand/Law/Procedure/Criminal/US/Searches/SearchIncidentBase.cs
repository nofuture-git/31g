using System;
using NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Searches
{
    /// <summary>
    /// Base type to isolate each child type&apos;s predicates
    /// </summary>
    public abstract class SearchIncidentBase : LegalConcept, ISearch
    {
        public Func<ILegalPerson[], ILegalPerson> GetConductorOfSearch { get; set; }

        public Func<ILegalPerson[], ILegalPerson> GetSubjectOfSearch { get; set; }

        public Arrest Arrest { get; set; }

        protected bool IsValidBeforePredicates(ILegalPerson[] persons)
        {
            if (Arrest == null)
            {
                AddReasonEntry($"{nameof(Arrest)} is unassigned");
                return false;
            }

            if (GetSubjectOfSearch == null)
                GetSubjectOfSearch = lps => null;

            if (GetConductorOfSearch == null)
                GetConductorOfSearch = lps => null;

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

            var suspect = GetSubjectOfSearch(persons);

            if (suspect == null)
            {
                AddReasonEntry($"{nameof(GetSubjectOfSearch)} returned nothing");
                return false;
            }
            var officer = GetConductorOfSearch(persons);
            if (officer == null)
            {
                AddReasonEntry($"{nameof(GetConductorOfSearch)} returned nothing");
                return false;
            }

            return true;

        }
    }
}
