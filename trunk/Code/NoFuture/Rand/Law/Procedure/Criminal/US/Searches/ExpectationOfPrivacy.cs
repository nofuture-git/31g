using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Criminal.US.Searches
{
    public class ExpectationOfPrivacy : LegalConcept, ISearch
    {
        public Func<ILegalPerson[], ILegalPerson> GetConductorOfSearch { get; set; } = lps => null;

        public Func<ILegalPerson[], ILegalPerson> GetSubjectOfSearch { get; set; } = lps => null;

        /// <summary>
        /// with consent law enforcement may search without any justification
        /// to whatever scope the consenting person gives.
        /// </summary>
        /// <remarks>
        /// When a 3rd party has apparent or actual authority then they may consent at the lose of the absentee
        /// </remarks>
        public IConsent Consent { get; set; }

        /// <summary>
        /// The subjective test being if the person-searched was expecting privacy
        /// </summary>
        public SubjectivePredicate<ILegalPerson> IsPrivacyExpected { get; set; } = lp => false;

        /// <summary>
        /// The objective test being if the a reasonable person would&apos;ve expected privacy
        /// </summary>
        /// <remarks>
        /// Don&apos;t expect privacy for anything which could be observed by a good detective or a neighbor
        /// </remarks>
        /// <remarks>
        /// Enhancement devices are limited to those in common public use - advanced spy-craft tech is not allowed.
        /// </remarks>
        public ObjectivePredicate<ILegalPerson> IsPrivacyExpectedReasonable { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (Consent != null && Consent.IsValid(persons))
            {
                AddReasonEntry($"{nameof(Consent)} {nameof(IsValid)} is true");
                AddReasonEntryRange(Consent.GetReasonEntries());
                return true;
            }

            var subjectPerson = GetSubjectOfSearch(persons);

            if (subjectPerson == null)
            {
                AddReasonEntry($"{nameof(GetSubjectOfSearch)} returned nothing");
                return false;
            }

            var title = subjectPerson.GetLegalPersonTypeName();

            if (!IsPrivacyExpected(subjectPerson))
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsPrivacyExpected)} is false");
                return false;
            }

            if (!IsPrivacyExpectedReasonable(subjectPerson))
            {
                AddReasonEntry($"{title} {subjectPerson.Name}, {nameof(IsPrivacyExpectedReasonable)} is false");
                return false;
            }

            return true;
        }

    }
}
