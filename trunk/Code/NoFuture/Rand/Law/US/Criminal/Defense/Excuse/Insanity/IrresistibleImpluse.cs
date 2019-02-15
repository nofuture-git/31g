using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse.Insanity
{
    /// <summary>
    /// similar to <see cref="MNaghten"/> only its considered simplier to prove and 
    /// is rejected in most jurisdictions
    /// </summary>
    public class IrresistibleImpluse : MNaghten
    {
        public IrresistibleImpluse(ICrime crime) : base(crime)
        {
        }
        /// <summary>
        /// Idea that the defendant can not control their conduct because of the mental defect
        /// </summary>
        [Aka("free choice")]
        public Predicate<ILegalPerson> IsVolitional { get; set; } = lp => true;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (IsVolitional(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsVolitional)} is true");
                return false;
            }

            return base.IsValid(offeror, offeree);
        }
    }
}
