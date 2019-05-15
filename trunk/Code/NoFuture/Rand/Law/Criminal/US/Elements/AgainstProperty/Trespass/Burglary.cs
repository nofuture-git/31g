using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Trespass
{
    /// <summary>
    /// unlawful entry into almost any structure or vehicle at any time
    /// </summary>
    [EtymologyNote("Old German", "'burg', 'baurgs'", "castle, city")]
    public class Burglary : CriminalTrespass
    {
        public Predicate<ILegalProperty> IsStructuredEnclosure { get; set; } = lp => false;

        /// <summary>
        /// any physical force used to enter 
        /// </summary>
        public Predicate<ILegalPerson> IsBreakingForce { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (IsBreakingForce != null && !IsBreakingForce(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsTangibleEntry)} is false");
                return false;
            }

            if (!IsStructuredEnclosure(SubjectProperty))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsStructuredEnclosure)} is false");
                return false;
            }

            return true;
        }

        public override bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var validIntend = criminalIntent is Purposely || criminalIntent is SpecificIntent ||
                              criminalIntent is Knowingly || criminalIntent is GeneralIntent;

            if (!validIntend)
            {
                AddReasonEntry($"{nameof(Burglary)} requires intent " +
                               $"of {nameof(Purposely)}, {nameof(SpecificIntent)}, " +
                               $"{nameof(Knowingly)}, {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
