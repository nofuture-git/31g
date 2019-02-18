using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse.Insanity
{
    /// <summary>
    /// A combination of <see cref="MNaghten"/> and <see cref="IrresistibleImpluse"/> 
    /// where each predicate is scaled down to only be substantial capacity instead of 
    /// total capacity see (Model Penal Code § 4.01(1)) 
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// was in greater use but after John Hinckley's assassination attempt 
    /// of President Reagan - states and fed went back to M'Naghten.
    /// ]]>
    /// </remarks>
    [Aka("ALI defense")]
    public class SubstantialCapacity : InsanityBase
    {
        public SubstantialCapacity(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// Must lack substantial, not total, capacity to know the difference between right and wrong.
        /// <![CDATA[
        /// When appreciate is the standard, must analyze the defendant’s emotional state, 
        /// character or personality as relevant and admissible.
        /// ]]>
        /// </summary>
        [Aka("appreciate")]
        public Predicate<ILegalPerson> IsMostlyWrongnessOfAware { get; set; } = lp => true;

        /// <summary>
        /// Must lack substantial, not total, ability to conform conduct to the requirements of the law.
        /// </summary>
        public Predicate<ILegalPerson> IsMostlyVolitional { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;
            var defendant = Crime.GetDefendant(persons);
            if (defendant == null)
                return false;

            if (IsMostlyVolitional(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMostlyVolitional)} is true");
                return false;
            }

            if (IsMostlyWrongnessOfAware(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMostlyWrongnessOfAware)} is true");
                return false;
            }

            return true;
        }
    }
}
