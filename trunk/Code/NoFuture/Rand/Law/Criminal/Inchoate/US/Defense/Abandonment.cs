using System;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.Inchoate.US.Defense
{
    /// <inheritdoc cref="IAbandonment"/>
    public class Abandonment : InchoateDefenseBase, IAbandonment
    {
        public Abandonment(ICrime crime) : base(crime)
        {
        }

        public Predicate<ILegalPerson> IsMotivatedByFearOfGettingCaught { get; set; } = lp => true;

        public Predicate<ILegalPerson> IsMotivatedByNewDifficulty { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!TestIsActusReusOfType(typeof(Attempt)))
                return false;

            if (IsMotivatedByFearOfGettingCaught(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMotivatedByFearOfGettingCaught)} is true");
                return false;
            }

            if (IsMotivatedByNewDifficulty(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMotivatedByNewDifficulty)} is true");
                return false;
            }

            return true;
        }
    }
}
