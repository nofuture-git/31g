using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
{
    /// <inheritdoc cref="IDefenseOfOthers" />
    public class DefenseOfOthers : DefenseOfBase, IDefenseOfOthers
    {
        public SubjectivePredicate<ILegalPerson> IsReasonablyAppearedInjuryOrDeath { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;
            if (!base.IsValid(persons))
                return false;

            if (!IsReasonablyAppearedInjuryOrDeath(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsReasonablyAppearedInjuryOrDeath)} is false");
                return false;
            }

            return true;
        }
    }
}
