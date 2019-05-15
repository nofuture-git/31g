using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Inchoate;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPersons
{
    /// <summary>
    /// Attempting to make physical contact but does not 
    /// </summary>
    public class AttemptedBattery : Attempt, IDominionOfForce, IElement
    {
        public Predicate<ILegalPerson> IsPresentAbility { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsApparentAbility { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            return base.IsValid(persons) || IsPresentAbility(defendant) || IsApparentAbility(defendant);
        }
    }
}
