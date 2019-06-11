using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements.ReasonableCare
{
    /// <inheritdoc />
    /// <summary>
    /// In order to meet the standard of ordinary duty under the law, those
    /// with uncommon physical limitations must exercise
    /// more diligence (e.g. blind, deaf, lame, feeble, etc.).
    /// </summary>
    public class OfPhysicalDeficiency : ReasonableCareBase
    {
        public OfPhysicalDeficiency(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// E.g. a cane, wheelchair, seeing-eye dog, etc.
        /// </summary>
        public Predicate<ILegalPerson> IsUsingCompensatoryDevice { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsAfflictedWith { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subjPerson = GetSubjectPerson(persons);

            if (subjPerson == null)
                return false;
            var title = subjPerson.GetLegalPersonTypeName();

            if (!IsAfflictedWith(subjPerson))
            {
                AddReasonEntry($"{title} {subjPerson.Name}, {nameof(IsAfflictedWith)} is false");
                return false;
            }

            if (!IsUsingCompensatoryDevice(subjPerson))
            {
                AddReasonEntry($"{title} {subjPerson.Name}, {nameof(IsUsingCompensatoryDevice)} is false");
                return false;
            }

            return true;
        }
    }
}
