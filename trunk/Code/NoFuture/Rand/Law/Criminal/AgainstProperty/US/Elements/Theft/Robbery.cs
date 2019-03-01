using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.HominiLupus.US;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    public class Robbery : ByTaking, IBattery, IAssault
    {
        public Predicate<ILegalPerson> IsByForce { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsByThreatOfForce { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!FromVictimPersonOrPresence(persons))
                return false;

            if (!base.IsValid(persons))
                return false;

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;
            var byForce = IsByForce(defendant);
            var byThreat = IsByThreatOfForce(defendant);

            if (!byForce && !byThreat)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByForce)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsByThreatOfForce)} is false");
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

            var victims = persons.Where(lp => lp is IVictim).ToList();
            if (!victims.Any())
            {
                AddReasonEntry(
                    $"there are not victims amound persons {string.Join(",", LegalPerson.GetNames(persons))}");
                return false;
            }

            if (SubjectOfTheft?.InPossessionOf == null)
            {
                AddReasonEntry($"the {nameof(SubjectOfTheft)}, {nameof(SubjectOfTheft.InPossessionOf)} is null");
                return false;
            }

            var possess = SubjectOfTheft.InPossessionOf;

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
