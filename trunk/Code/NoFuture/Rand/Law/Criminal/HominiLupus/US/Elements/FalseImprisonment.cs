using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    [Aka("felonious restraint")]
    public class FalseImprisonment : LegalConcept, IActusReus
    {
        public Predicate<ILegalPerson> IsConfineVictim { get; set; } = lp => false;

        public IConsent Consent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;
            if (!IsConfineVictim(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsConfineVictim)} is false");
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
