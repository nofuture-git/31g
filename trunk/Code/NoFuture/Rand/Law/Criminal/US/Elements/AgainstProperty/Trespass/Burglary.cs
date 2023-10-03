using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstProperty.Trespass
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

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (IsBreakingForce != null && !IsBreakingForce(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsTangibleEntry)} is false");
                return false;
            }

            if (!IsStructuredEnclosure(SubjectProperty))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsStructuredEnclosure)} is false");
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
