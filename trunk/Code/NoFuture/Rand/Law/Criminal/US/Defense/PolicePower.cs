using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense
{
    /// <inheritdoc cref="IPolicePower"/>
    public class PolicePower : DefenseBase, IPolicePower
    {
        public Predicate<ILegalPerson> IsAgentOfTheState { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsReasonableUseOfForce { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsAgentOfTheState(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsAgentOfTheState)} is false");
                return false;
            }

            if (!IsReasonableUseOfForce(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsReasonableUseOfForce)} is false");
                return false;
            }

            return true;
        }
    }
}
