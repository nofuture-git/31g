using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Inchoate
{
    /// <summary>
    /// an agreement to commit any criminal offense
    /// </summary>
    public class Conspiracy : LegalConcept, IActusReus
    {
        public Predicate<ILegalPerson> IsAgreementToCommitCrime { get; set; } = lp => false;

        /// <summary>
        /// In some states and federally an overt act in furtherance of the conspiracy is required.
        /// It does not need to, itself, be criminal - just preparatory.
        /// </summary>
        public Predicate<ILegalPerson> IsOvertActRequired { get; set; }

        /// <summary>
        /// there cannot be a conspiracy for a crime which requires two people (e.g. prostitution)
        /// </summary>
        [Aka("Wharton's rule")]
        public bool IsConcertOfAction { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (IsConcertOfAction)
            {
                AddReasonEntry("there cannot be a conspiracy for a crime which " +
                               "requires two people (e.g. prostitution)");
                return false;
            }

            var defendant = persons.Defendant();
            if (defendant == null)
                return false;
            if (!IsAgreementToCommitCrime(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsAgreementToCommitCrime)} is false");
                return false;
            }

            if (IsOvertActRequired != null && !IsOvertActRequired(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsOvertActRequired)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isRequiredForConspriacy = criminalIntent is Purposely || criminalIntent is SpecificIntent;
            if (!isRequiredForConspriacy)
            {
                AddReasonEntry($"criminal intent element required for {nameof(Conspiracy)} is specific intent or purposely");
                return false;
            }

            return true;
        }
    }
}
