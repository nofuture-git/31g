using System;
using System.Linq;

namespace NoFuture.Law.US
{
    public class Consent : UnoHomine, IConsent
    {
        public Consent() :this(ExtensionMethods.Plaintiff) { } 

        public Consent(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public static IConsent NotGiven()
        {
            return new No();
        }

        public static IConsent IsGiven()
        {
            return new Yes();
        }

        private class No : Consent
        {
            public override bool IsValid(params ILegalPerson[] persons)
            {
                return false;
            }
        }

        private class Yes : Consent
        {
            public override bool IsValid(params ILegalPerson[] persons)
            {
                return true;
            }
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
