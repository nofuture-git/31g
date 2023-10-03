using System;
using System.Linq;
using NoFuture.Law.Criminal.US.Elements.AgainstProperty.Theft;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Criminal.US.Elements.AttendantCircumstances
{
    /// <summary>
    /// An attendant circumstance in which the <see cref="IVictim"/> was not only 
    /// deceived but also relied upon that deception.
    /// </summary>
    public class Reliance : AttendantCircumstanceBase
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

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var victims = persons.Victims().ToList();
            if (!victims.Any())
                return false;

            foreach (var victim in victims)
            {
                var isReliance = IsReliantOnFalseRepresentation(victim);
                var victimTitle = victim.GetLegalPersonTypeName();
                if(isReliance)
                    AddReasonEntry($"{victimTitle} {victim.Name}, " +
                                   $"{nameof(IsReliantOnFalseRepresentation)} is {isReliance}");
                return isReliance;
            }

            return false;
        }
    }
}
