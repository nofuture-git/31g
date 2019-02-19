using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// the facts as the defendant believes them to be negate the requisite intent for the crime at issue
    /// </summary>
    public class MistakeOfFact : DefenseBase
    {
        public MistakeOfFact(ICrime crime) : base(crime)
        {
        }

        public SubjectivePredicate<ILegalPerson> IsBeliefNegateIntent { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = Crime.GetDefendant(persons);
            if (defendant == null)
                return false;

            if (Crime.Concurrence.MensRea is StrictLiability)
            {
                AddReasonEntry($"defendant, {defendant.Name}, mens rea is {nameof(StrictLiability)}");
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
