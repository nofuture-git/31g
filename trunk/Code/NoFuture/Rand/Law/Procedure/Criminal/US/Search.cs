using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// The right of the people to be secure in their persons, houses, papers,
    /// and effects, against unreasonable search and seizures, shall not be
    /// violated, and no Warrants shall issue, but upon probable cause
    /// </summary>
    public class Search : LegalConcept, ISearch
    {

        /// <summary>
        /// Resolves the person who conducted the search - they must be a government 
        /// </summary>
        public Func<ILegalPerson[], ILegalPerson> GetConductorOfSearch { get; set; } = lps => null;


        /// <summary>
        /// Resolve the place searched as some such-and-such name
        /// </summary>
        public Func<ILegalPerson, IVoca> GetSubjectOfSearch { get; set; } = lp => null;

        /// <summary>
        /// Fourth Amendment only applies to governmental actors or persons acting at the direction of the government.
        /// </summary>
        public Predicate<ILegalPerson> IsAgentOfTheState { get; set; } = lp => lp is IGovernment;

        /// <summary>
        /// persons acting at the direction of the government or pursuant to government interest
        /// </summary>
        public ISearch InstrumentOfTheState { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var searchedBy = GetConductorOfSearch(persons);
            if (searchedBy == null)
            {
                AddReasonEntry($"{nameof(GetConductorOfSearch)} returned nothing");
                return false;
            }

            var searchByTitle = searchedBy.GetLegalPersonTypeName();

            if (!IsAgentOfTheState(searchedBy))
            {
                AddReasonEntry($"{searchByTitle} {searchedBy.Name}, {nameof(IsAgentOfTheState)} is false");
                return false;
            }

            var isGovtAgent = IsAgentOfTheState(searchedBy);

            if (InstrumentOfTheState != null && InstrumentOfTheState.GetConductorOfSearch == null)
            {
                InstrumentOfTheState.GetConductorOfSearch = GetConductorOfSearch;
            }

            if (!isGovtAgent && !(InstrumentOfTheState?.IsValid(persons) ?? true))
            {
                AddReasonEntry($"{searchByTitle} {searchedBy.Name}, {nameof(IsAgentOfTheState)} is false");
                AddReasonEntryRange(InstrumentOfTheState.GetReasonEntries());
                return false;
            }

            throw new NotImplementedException();
        }

    }
}
