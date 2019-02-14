using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Justification
{
    public class DefenseOfProperty : DefenseOfBase
    {
        public DefenseOfProperty(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// This is objective OR subjective based on jurisdiction
        /// </summary>
        public Predicate<ILegalPerson> IsBeliefProtectProperty { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;
            if (!base.IsValid(offeror, offeree))
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
