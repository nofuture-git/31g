using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense.Insanity;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <inheritdoc cref="IMNaghten" />
    public class MNaghten : InsanityBase, IMNaghten
    {
        public Predicate<ILegalPerson> IsNatureQualityOfAware { get; set; } = lp => true;

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
