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
    /// deceived but also relied upon that deception
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
            var falsePretense = criminalAct as ByDeception;
            if(falsePretense == null || persons == null || !persons.Any())
                return base.IsValid(criminalAct, persons);

            var victims = persons.Where(lp => lp is IVictim).ToList();
            if (!victims.Any())
                return false;

            foreach (var victim in victims)
            {
                var isReliance = IsReliantOnFalseRepresentation(victim);
                AddReasonEntry($"victim, {victim.Name}, had property {falsePretense.SubjectOfTheft} theft {nameof(ByDeception)}; " +
                               $"furthermore, {nameof(IsReliantOnFalseRepresentation)} is {isReliance}");
                if (isReliance)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
