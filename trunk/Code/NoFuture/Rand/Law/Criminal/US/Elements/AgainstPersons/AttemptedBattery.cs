using System;
using NoFuture.Law.Criminal.US.Elements.Inchoate;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPersons
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
            if (!base.IsValid(persons))
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();

            if (!IsPresentAbility(defendant) && !IsApparentAbility(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsPresentAbility)} " +
                               $"and {nameof(IsApparentAbility)} are both false");
                return false;
            }

            return true;
        }
    }
}
