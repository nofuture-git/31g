using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <inheritdoc cref="ISexBipartitie"/>
    /// <inheritdoc cref="IAssault"/>
    /// <inheritdoc cref="IBattery"/>
    [EtymologyNote("Latin", "'rapere'", "to steal or seize")]
    public class Rape : SexBipartitie, IAssault, IBattery, IActusReus, IElement
    {
        public Predicate<ILegalPerson> IsByViolence { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsByThreatOfViolence { get; set; } = lp => false;

        public IConsent Consent { get; set; } = new VictimConsent();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!base.IsValid(persons))
                return false;

            var isByForce = IsByViolence(defendant);
            var isByThreatOfForce = IsByThreatOfViolence(defendant);
            if (isByForce || isByThreatOfForce)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByViolence)} is {isByForce}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByThreatOfViolence)} is {isByThreatOfForce}");
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
