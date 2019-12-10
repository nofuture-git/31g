using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// The concept of a private party acting as an instrument of the
    /// state - such as being directed by an agent of the state or pursuant
    /// to an official policy
    /// </summary>
    public class InstrumentOfTheState : LegalConcept, ISearch
    {

        public Func<ILegalPerson[], ILegalPerson> GetConductorOfSearch { get; set; }

        /// <summary>
        /// the degree of government encouragement, knowledge, and or
        /// acquiescence with regard to the private actor&apos;s conduct
        /// </summary>
        public Predicate<ILegalPerson> IsAcquiescenceOfTheState { get; set; } = lp => false;

        /// <summary>
        /// search is to promote some government interest such as discovery of
        /// criminal activity or evidence thereof
        /// </summary>
        public Predicate<ILegalPerson> IsPromoteInterestOfTheState { get; set; } = lp => false;


        /// <summary>
        /// Fourth Amendment only applies to governmental actors or persons acting at the direction of the government.
        /// </summary>
        public Predicate<ILegalPerson> IsAgentOfTheState { get; set; } = lp => lp is IGovernment;

        /// <summary>
        /// Determines if the Fourth Amendment is applicable to the search conducted by a non-state agent.
        /// </summary>
        /// <returns>True if the Fourth Amendment protection apply</returns>
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var getConductorOfSearch = GetConductorOfSearch ?? (lps => null);
            var subjectPerson = getConductorOfSearch(persons);
            if (subjectPerson == null)
            {
                AddReasonEntry($"{nameof(GetConductorOfSearch)} returned nothing");
                return false;
            }

            var title = subjectPerson.GetLegalPersonTypeName();

            if (IsAgentOfTheState(subjectPerson))
            {
                return true;
            }

            if (!IsAcquiescenceOfTheState(subjectPerson))
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsAgentOfTheState)} is false");
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsAcquiescenceOfTheState)} is false");
                return false;
            }

            if (!IsPromoteInterestOfTheState(subjectPerson))
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsAgentOfTheState)} is false");
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsPromoteInterestOfTheState)} is false");
                return false;
            }

            return true;
        }
    }
}
