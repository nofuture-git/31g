using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IEntrapment"/>
    public class Entrapment : DefenseBase, IEntrapment
    {
        public Predicate<ILegalPerson> IsIntentOriginFromLawEnforcement { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsIntentOriginFromLawEnforcement(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, mens rea is {nameof(IsIntentOriginFromLawEnforcement)}");
                return false;
            }

            return true;
        }
    }
}
