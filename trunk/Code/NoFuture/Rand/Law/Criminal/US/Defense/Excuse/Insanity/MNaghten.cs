using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense.Insanity;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <inheritdoc cref="IMNaghten" />
    public class MNaghten : InsanityBase, IMNaghten
    {
        public MNaghten(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public MNaghten() : this(ExtensionMethods.Defendant) { }

        public Predicate<ILegalPerson> IsNatureQualityOfAware { get; set; } = lp => true;

        public Predicate<ILegalPerson> IsWrongnessOfAware { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;

            var isKnowNatureOf = IsNatureQualityOfAware(legalPerson);
            var isWrongnessOf = IsWrongnessOfAware(legalPerson);

            if (isKnowNatureOf && isWrongnessOf)
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsNatureQualityOfAware)} is {isKnowNatureOf} " +
                               $"and {nameof(IsWrongnessOfAware)} is {isWrongnessOf}");
                return false;
            }

            return true;
        }
    }
}
