using System;
using System.Linq;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements
{
    /// <summary>
    /// An attendant circumstance in which the <see cref="IVictim"/> was not only 
    /// deceived but also relied upon that deception.
    /// </summary>
    public class Reliance : AttendantCircumstances
    {
        public Predicate<ILegalPerson> IsReliantOnFalseRepresentation { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            return false;
        }

        public override bool IsValid(IActusReus criminalAct, params ILegalPerson[] persons)
        {
            var theft = criminalAct as ByDeception;
            if(theft == null || persons == null || !persons.Any())
                return base.IsValid(criminalAct, persons);

            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var victims = GetVictims(persons);
            if (!victims.Any())
                return false;

            foreach (var victim in victims)
            {
                var isReliance = IsReliantOnFalseRepresentation(victim);
                if(isReliance)
                    AddReasonEntry($"victim, {victim.Name}, {nameof(IsReliantOnFalseRepresentation)} is {isReliance}");
                return isReliance;
            }

            return false;
        }
    }
}
