using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements
{
    /// <summary>
    /// unlawful entry into almost any structure or vehicle at any time
    /// </summary>
    [Note("burg: german 'castle'", "baurgs: german 'city'")]
    public class Burglary : AgitPropertyBase, IActusReus
    {
        public Predicate<ILegalProperty> IsStructuredEnclosure { get; set; } = lp => false;

        /// <summary>
        /// any physical force used to enter 
        /// </summary>
        public Predicate<ILegalPerson> IsBreakingForce { get; set; }

        /// <summary>
        /// partial or complete intrusion of either the defendant, the defendant&apos;s body part or a tool or instrument
        /// </summary>
        public Predicate<ILegalPerson> IsEntry { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (!GetConsent(persons))
                return false;

            if (IsBreakingForce != null && !IsBreakingForce(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsEntry)} is false");
                return false;
            }

            if (!IsStructuredEnclosure(SubjectProperty))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsStructuredEnclosure)} is false");
                return false;
            }

            if (!IsEntry(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsEntry)} is false");
                return false;
            }

            return true;
        }

        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntend = criminalIntent is Purposely || criminalIntent is SpecificIntent ||
                              criminalIntent is Knowingly || criminalIntent is GeneralIntent;

            if (!validIntend)
            {
                AddReasonEntry($"{nameof(ByTaking)} requires intent " +
                               $"of {nameof(Purposely)}, {nameof(SpecificIntent)}, " +
                               $"{nameof(Knowingly)}, {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
