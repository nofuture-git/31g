using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <inheritdoc cref="IBattery"/>
    /// <summary>
    /// Same as <see cref="AttemptedBattery"/> except there is actual touch\contact of some kind
    /// </summary>
    public class Battery : CriminalBase, IBattery, IActusReus
    {
        public Predicate<ILegalPerson> IsByForce { get; set; } = lp => false;

        /// <summary>
        /// If the victim entered into Mutual Combat then its not battery
        /// </summary>
        public Consent Consent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsByForce(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByForce)} is false");
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
