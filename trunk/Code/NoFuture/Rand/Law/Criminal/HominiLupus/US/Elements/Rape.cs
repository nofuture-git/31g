using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <inheritdoc cref="IBipartite"/>
    /// <inheritdoc cref="IAssault"/>
    /// <inheritdoc cref="IBattery"/>
    [EtymologyNote("Latin", "''rapere", "to steal or seize")]
    public class Rape : CriminalBase, IBipartite, IAssault, IBattery, IActusReus
    {
        public Predicate<ILegalPerson> IsSexualIntercourse { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsByForce { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsByThreatOfForce { get; set; } = lp => false;

        public IConsent Consent { get; set; } = new Consent();

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!IsSexualIntercourse(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsSexualIntercourse)} is false");
                return false;
            }

            var isByForce = IsByForce(defendant);
            var isByThreatOfForce = IsByThreatOfForce(defendant);
            if (isByForce || isByThreatOfForce)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByForce)} is {isByForce}");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByThreatOfForce)} is {isByThreatOfForce}");
                return true;
            }
            var isConsented = Consent?.IsValid(persons) ?? false;

            AddReasonEntryRange(Consent?.GetReasonEntries());
            return !isConsented;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            var consent = Consent as Consent ?? new Consent();

            foreach (var person in persons)
            {
                var victim = person as IVictim;
                if(victim == null)
                    continue;

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
