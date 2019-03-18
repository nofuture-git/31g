using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    public class Robbery : ByTaking, IBattery, IAssault, IElement
    {
        public Predicate<ILegalPerson> IsByViolence { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsByThreatOfViolence { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!FromVictimPersonOrPresence(persons))
                return false;

            if (!base.IsValid(persons))
                return false;

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;
            var byForce = IsByViolence(defendant);
            var byThreat = IsByThreatOfViolence(defendant);

            if (!byForce && !byThreat)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByViolence)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByThreatOfViolence)} is false");
                return false;
            }

            return true;
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntend = criminalIntent is Purposely || criminalIntent is SpecificIntent;

            if (!validIntend)
            {
                AddReasonEntry($"{nameof(ByTaking)} requires intent " +
                               $"of {nameof(Purposely)}, {nameof(SpecificIntent)}");
                return false;
            }

            return true;
        }


        /// <summary>
        /// <![CDATA[ The property must be taken from the victim's person or presensece - meaning it was under the victims control. ]]>
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        protected virtual bool FromVictimPersonOrPresence(ILegalPerson[] persons)
        {
            if (persons == null || !persons.Any())
                return false;

            var victims = persons.Where(p => IsVictim(p)).ToList();
            if (!victims.Any())
            {
                AddReasonEntry(
                    $"there are not victims among persons {string.Join(",", LegalPerson.GetNames(persons))}");
                return false;
            }

            if (!TryGetPossesorOfProperty(out var possess))
                return false;

            foreach (var victim in victims)
            {
                if (possess.Equals(victim) || ReferenceEquals(possess, victim))
                    return true;
                AddReasonEntry($"victim, {victim.Name}, is not in possession of {possess.GetType().Name} '{possess.Name}'");
            }

            return false;
        }
    }
}
