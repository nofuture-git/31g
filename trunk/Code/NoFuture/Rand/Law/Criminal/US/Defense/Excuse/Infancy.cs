using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IAgeOfMajority"/>
    public class Infancy: DefenseBase, IAgeOfMajority
    {
        public Predicate<ILegalPerson> IsUnderage { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var isAdult = !IsUnderage(defendant);

            if (isAdult)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUnderage)} is false");
                return false;
            }

            return true;
        }
    }
}
