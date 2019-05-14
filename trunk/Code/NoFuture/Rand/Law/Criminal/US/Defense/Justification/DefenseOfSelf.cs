using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <inheritdoc cref="IDefenseOfSelf"/>
    public class DefenseOfSelf : DefenseOfBase, IDefenseOfSelf
    {
        public ObjectivePredicate<ILegalPerson> IsReasonableFearOfInjuryOrDeath { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;
            if (!base.IsValid(persons))
                return false;

            if (!IsReasonableFearOfInjuryOrDeath(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsReasonableFearOfInjuryOrDeath)} is false");
                return false;
            }

            return true;
        }
    }
}
