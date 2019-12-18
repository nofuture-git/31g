using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.AttendantCircumstances;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPersons
{
    /// <inheritdoc cref="ISexBilateral"/>
    /// <inheritdoc cref="IAssault"/>
    /// <inheritdoc cref="IBattery"/>
    [EtymologyNote("Latin", "'rapere'", "to steal or seize")]
    public class Rape : SexBilateral, IAssault, IBattery, IActusReus, IElement
    {
        public Predicate<ILegalPerson> IsByViolence { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsByThreatOfViolence { get; set; } = lp => false;

        public IConsent Consent { get; set; } = new VictimConsent();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            var isByForce = IsByViolence(defendant);
            var isByThreatOfForce = IsByThreatOfViolence(defendant);
            if (isByForce || isByThreatOfForce)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsByViolence)} is {isByForce}");
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsByThreatOfViolence)} is {isByThreatOfForce}");
                return true;
            }

            var isConsented = Consent?.IsValid(persons) ?? false;

            AddReasonEntryRange(Consent?.GetReasonEntries());
            return !isConsented;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            var consent = Consent as VictimConsent ?? new VictimConsent();

            foreach (var victim in persons.Victims())
            {
                var isCapable = consent.IsCapableThereof(victim);
                var isIntercourse = IsSexualIntercourse(defendant);

                //the intent is irrelevant - sex with victim is illegal outright
                var isSexWithIllegal = defendant != null
                                       && criminalIntent is StrictLiability
                                       && !isCapable
                                       && isIntercourse;

                if (isSexWithIllegal)
                {
                    AddReasonEntry($"{nameof(IsSexualIntercourse)} is true with victim, {victim.Name}, " +
                                   $"who is {nameof(consent.IsCapableThereof)} is false " +
                                   $"for {nameof(Consent)} while intent is {nameof(StrictLiability)}");
                    AddReasonEntry($"sex between {victim.Name} and defendant, {defendant.Name} is illegal outright.");
                }
            }
            return true;
        }
    }
}
