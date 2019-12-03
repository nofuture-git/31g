using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    public class ExpectationOfPrivacy : LegalConcept, ISearch
    {
        public Func<ILegalPerson[], ILegalPerson> GetConductorOfSearch { get; set; } = lps => null;

        public Func<ILegalPerson[], ILegalPerson> GetSubjectOfSearch { get; set; } = lps => null;

        public SubjectivePredicate<ILegalPerson> IsPrivacyExpected { get; set; } = lp => false;

        public ObjectivePredicate<ILegalPerson> IsPrivacyExpectedReasonable { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
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
