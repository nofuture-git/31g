using System;
using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    public class Consent : LegalConcept, IConsent
    {
        protected internal Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; set; }

        public Consent() :this(null) { }

        public Consent(Func<ILegalPerson[], ILegalPerson> getSubjectPerson)
        {
            GetSubjectPerson = getSubjectPerson ?? ExtensionMethods.Plaintiff;
        }

        public Predicate<ILegalPerson> IsCapableThereof { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (persons == null || !persons.Any())
            {
                AddReasonEntry($"{nameof(persons)} is null or empty");
                return false;
            }

            var person = GetSubjectPerson(persons);
            if (person == null)
                return false;
            var title = person.GetLegalPersonTypeName();

            if (!IsCapableThereof(person))
            {
                AddReasonEntry($"{title}, {person.Name}, {nameof(Consent)} {nameof(IsCapableThereof)} is false");
                return false;
            }

            if (!IsApprovalExpressed(person))
            {
                AddReasonEntry($"{title}, {person.Name}, {nameof(Consent)} {nameof(IsApprovalExpressed)} is false");
                return false;
            }

            return true;
        }
    }
}
