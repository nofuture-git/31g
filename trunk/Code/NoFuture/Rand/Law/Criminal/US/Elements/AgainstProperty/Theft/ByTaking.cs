using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty.Theft
{
    /// <summary>
    /// A union of the various ways of directly stealing;  Model Penal Code 223.2. and 223.7.
    /// </summary>
    [Aka("larceny", "conversion")]
    public class ByTaking : ConsolidatedTheft
    {
        /// <summary>
        /// The idea of, once having taken control over the property, the defendant attempts to move
        /// it, even slightly.
        /// </summary>
        [Aka("movement", "carrying away")]
        public Predicate<ILegalPerson> IsAsportation { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsAsportation(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsAsportation)} is false");
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
                AddReasonEntry($"{nameof(ByTaking)} requires intent " +
                               $"of {nameof(Purposely)}, {nameof(SpecificIntent)}, " +
                               $"{nameof(Knowingly)}, {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }
    }
}
