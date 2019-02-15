using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse
{

    /// <summary>
    /// Insanity defense named after Daniel M'Naghten from England (1843).
    /// </summary>
    [Aka("right-wrong test")]
    public class MNaghten : Insanity
    {
        public MNaghten(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// Having a basic level of awareness under the attendant circumstances
        /// </summary>
        public Predicate<ILegalPerson> IsNatureQualityOfAware { get; set; } = lp => true;

        /// <summary>
        /// to assert that they did not know their act was wrong
        /// </summary>
        public Predicate<ILegalPerson> IsWrongnessOfAware { get; set; } = lp => true;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (!base.IsValid(offeror, offeree))
                return false;
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            var isKnowNatureOf = IsNatureQualityOfAware(defendant);
            var isWrongnessOf = IsWrongnessOfAware(defendant);

            if (isKnowNatureOf && isWrongnessOf)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsNatureQualityOfAware)} is {isKnowNatureOf} " +
                               $"and {nameof(IsWrongnessOfAware)} is {isWrongnessOf}");
                return false;
            }

            return true;
        }
    }
}
