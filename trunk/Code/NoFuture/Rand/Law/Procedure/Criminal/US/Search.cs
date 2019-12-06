using System;

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
        /// persons who are, themselves, government agents or persons acting
        /// at the direction of the government or pursuant to government interest
        /// </summary>
        public ISearch InstrumentOfTheState { get; set; }

        /// <summary>
        /// The person-who-is-searched&apos;s expectation of privacy
        /// </summary>
        public ISearch ExpectationOfPrivacy { get; set; }

        /// <summary>
        /// The legal reasoning for the suspicion.
        /// </summary>
        /// <remarks>
        /// Typical forms:probable cause, reasonable suspicion,
        /// administrative, consent, in-plain-view
        /// <![CDATA[
        /// ]]> </remarks>
        public ILegalConcept SearchReason { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            return TestIsAgentOrInstrumentOfState(persons)
                   && TestIsExpectationOfPrivacy(persons)
                   && TestSourceOfSuspicion(persons);
        }

        protected bool TestIsAgentOrInstrumentOfState(ILegalPerson[] persons)
        {
            if (InstrumentOfTheState == null)
            {
                InstrumentOfTheState = new InstrumentOfTheState();
            }

            //assign this if its missing
            if (InstrumentOfTheState.GetConductorOfSearch == null)
                InstrumentOfTheState.GetConductorOfSearch = GetConductorOfSearch;

            var isAgtOrInstrumentOfState = InstrumentOfTheState.IsValid(persons);

            AddReasonEntryRange(InstrumentOfTheState.GetReasonEntries());

            return isAgtOrInstrumentOfState;
        }

        protected virtual bool TestSourceOfSuspicion(ILegalPerson[] persons)
        {
            //search is defined as being state conducted in private space
            if (SearchReason == null)
                return true;

            var sosResult = SearchReason.IsValid(persons);
            AddReasonEntryRange(SearchReason.GetReasonEntries());
            return sosResult;
        }

        protected bool TestIsExpectationOfPrivacy(ILegalPerson[] persons)
        {
            if (ExpectationOfPrivacy == null)
            {
                AddReasonEntry($"{nameof(ExpectationOfPrivacy)} is unassigned");
                return false;
            }

            if (ExpectationOfPrivacy.GetConductorOfSearch == null)
                ExpectationOfPrivacy.GetConductorOfSearch = GetConductorOfSearch;

            var isExpectPrivacy = ExpectationOfPrivacy.IsValid(persons);

            AddReasonEntryRange(ExpectationOfPrivacy.GetReasonEntries());

            return isExpectPrivacy;
        }

    }
}
