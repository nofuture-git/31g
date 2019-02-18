using NoFuture.Rand.Law.US.Criminal.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse
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

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
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
