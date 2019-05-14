using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense.Insanity;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <inheritdoc cref="ISubstantialCapacity"/>
    public class SubstantialCapacity : InsanityBase, ISubstantialCapacity
    {
        public Predicate<ILegalPerson> IsMostlyWrongnessOfAware { get; set; } = lp => true;

        public Predicate<ILegalPerson> IsMostlyVolitional { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;
            var defendant = persons.Defendant();
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
