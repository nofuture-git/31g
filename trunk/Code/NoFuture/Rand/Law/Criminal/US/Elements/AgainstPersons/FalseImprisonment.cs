using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPersons
{
    [Aka("felonious restraint")]
    public class FalseImprisonment : LegalConcept, IActusReus
    {
        public Predicate<ILegalPerson> IsConfineVictim { get; set; } = lp => false;

        public IConsent Consent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsConfineVictim(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsConfineVictim)} is false");
                return false;
            }

            var isConsent = Consent?.IsValid(persons) ?? false;
            AddReasonEntryRange(Consent?.GetReasonEntries());

            return !isConsent;

        }

        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
