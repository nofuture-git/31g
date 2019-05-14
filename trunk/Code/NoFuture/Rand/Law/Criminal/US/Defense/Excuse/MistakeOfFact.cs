using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <inheritdoc cref="IMistakeOfFact"/>
    public class MistakeOfFact : DefenseBase, IMistakeOfFact
    {
        public SubjectivePredicate<ILegalPerson> IsBeliefNegateIntent { get; set; } = lp => false;

        public bool IsStrictLiability { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (IsStrictLiability)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsStrictLiability)} is true");
                return false;
            }

            if (!IsBeliefNegateIntent(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsBeliefNegateIntent)} is false");
                return false;
            }

            return true;
        }
    }
}
