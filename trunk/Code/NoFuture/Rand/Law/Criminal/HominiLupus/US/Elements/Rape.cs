using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <inheritdoc cref="ISexBipartitie"/>
    /// <inheritdoc cref="IAssault"/>
    /// <inheritdoc cref="IBattery"/>
    [EtymologyNote("Latin", "''rapere", "to steal or seize")]
    public class Rape : CriminalBase, ISexBipartitie, IAssault, IBattery, IActusReus, IElement
    {
        public Predicate<ILegalPerson> IsSexualIntercourse { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsByViolence { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsByThreatOfViolence { get; set; } = lp => false;

        public IConsent Consent { get; set; } = new Consent();

        public Predicate<ILegalPerson> IsOneOfTwo { get; set; } = lp => false;

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

            if (!IsOneOfTwo(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsOneOfTwo)} is false");
            }

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
            var defendant = GetDefendant(persons);
            var consent = Consent as Consent ?? new Consent();

            foreach (var victim in persons.Where(p => IsVictim(p)))
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
