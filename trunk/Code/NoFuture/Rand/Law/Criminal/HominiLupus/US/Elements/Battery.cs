using System;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
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
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsByViolence(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByViolence)} is false");
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
