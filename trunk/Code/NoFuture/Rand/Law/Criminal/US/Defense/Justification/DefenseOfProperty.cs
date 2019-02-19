using System;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Justification
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

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = Crime.GetDefendant(persons);
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
