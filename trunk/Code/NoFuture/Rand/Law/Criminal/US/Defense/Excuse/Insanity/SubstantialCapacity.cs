using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense.Insanity;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse.Insanity
{
    /// <inheritdoc cref="ISubstantialCapacity"/>
    public class SubstantialCapacity : InsanityBase, ISubstantialCapacity
    {
        public SubstantialCapacity(Func<ILegalPerson[], ILegalPerson> getSubjectPerson): base(getSubjectPerson) { }

        public SubstantialCapacity() : this(ExtensionMethods.Defendant) { }

        public Predicate<ILegalPerson> IsMostlyWrongnessOfAware { get; set; } = lp => true;

        public Predicate<ILegalPerson> IsMostlyVolitional { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;
            var legalPerson = persons.Defendant();
            if (legalPerson == null)
                return false;

            if (IsMostlyVolitional(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsMostlyVolitional)} is true");
                return false;
            }

            if (IsMostlyWrongnessOfAware(legalPerson))
            {
                AddReasonEntry($"{legalPerson.GetLegalPersonTypeName()}, {legalPerson.Name}, {nameof(IsMostlyWrongnessOfAware)} is true");
                return false;
            }

            return true;
        }
    }
}
