using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPersons
{
    /// <inheritdoc cref="IBattery"/>
    /// <summary>
    /// Same as <see cref="AttemptedBattery"/> except there is actual touch\contact of some kind
    /// </summary>
    public class Battery : LegalConcept, IBattery, IActusReus, IElement
    {
        public Predicate<ILegalPerson> IsByViolence { get; set; } = lp => false;

        /// <summary>
        /// If the victim entered into Mutual Combat then its not battery
        /// </summary>
        public IConsent Consent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            if (!IsByViolence(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsByViolence)} is false");
            }

            var isConsented = Consent?.IsValid(persons) ?? false;

            AddReasonEntryRange(Consent?.GetReasonEntries());
            return !isConsented;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }
    }
}
