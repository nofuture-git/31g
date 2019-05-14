using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <inheritdoc cref="IDefenseOfProperty"/>
    public class DefenseOfProperty : DefenseOfBase, IDefenseOfProperty
    {
        public Predicate<ILegalPerson> IsBeliefProtectProperty { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;
            if (!base.IsValid(persons))
                return false;
            if (!IsBeliefProtectProperty(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsBeliefProtectProperty)} is false");
                return false;
            }

            return true;
        }
    }
}
