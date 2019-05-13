using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// the facts as the defendant believes them to be negate the requisite intent for the crime at issue
    /// </summary>
    public class MistakeOfFact : DefenseBase
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
