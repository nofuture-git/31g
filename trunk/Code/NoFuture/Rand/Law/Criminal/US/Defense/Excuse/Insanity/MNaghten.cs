using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
{

    /// <summary>
    /// Insanity defense named after Daniel M'Naghten from England (1843).
    /// </summary>
    [Aka("right-wrong test")]
    public class MNaghten : InsanityBase
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

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;
            var defendant = persons.Defendant();
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
