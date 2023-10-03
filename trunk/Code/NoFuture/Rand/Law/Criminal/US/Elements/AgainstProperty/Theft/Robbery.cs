using System;
using System.Linq;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstProperty.Theft
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

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var byForce = IsByViolence(defendant);
            var byThreat = IsByThreatOfViolence(defendant);

            if (!byForce && !byThreat)
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsByViolence)} is false");
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsByThreatOfViolence)} is false");
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

            var victims = persons.Victims().ToList();
            if (!victims.Any())
            {
                AddReasonEntry(
                    $"there are not victims among persons {string.Join(",", LegalPerson.GetNames(persons))}");
                return false;
            }

            var possess = persons.FirstOrDefault(p => SubjectProperty.IsInPossessionOf(p));

            foreach (var victim in victims)
            {
                if (possess == null || possess.Equals(victim) || ReferenceEquals(possess, victim))
                    return true;
                
                AddReasonEntry($"{victim.GetLegalPersonTypeName()} {victim.Name}, " +
                               $"is not in possession of {possess.GetType().Name} '{possess.Name}'");
            }

            return false;
        }
    }
}
