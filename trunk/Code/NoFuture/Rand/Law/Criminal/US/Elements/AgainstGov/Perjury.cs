using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstGov
{
    /// <summary>
    /// Lying under oath
    /// </summary>
    public class Perjury : LegalConcept, IActusReus
    {
        public Predicate<ILegalPerson> IsFalseTestimony { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsUnderOath { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsJudicialProceeding { get; set; } = lp => false;

        /// <summary>
        /// an issue that must be decided in order to resolve a controversy
        /// </summary>
        public Predicate<ILegalPerson> IsMaterialIssue { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsFalseTestimony(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsFalseTestimony)} is false");
                return false;
            }

            if (!IsUnderOath(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsUnderOath)} is false");
                return false;
            }
            if (!IsJudicialProceeding(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsJudicialProceeding)} is false");
                return false;
            }
            if (!IsMaterialIssue(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsMaterialIssue)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isValid = criminalIntent is Purposely || criminalIntent is SpecificIntent
                            || criminalIntent is Knowingly;

            if (!isValid)
            {
                AddReasonEntry($"{nameof(Perjury)} requires intent of {nameof(Purposely)}, " +
                               $"{nameof(SpecificIntent)}, {nameof(Knowingly)}");
                return false;
            }

            return true;
        }
    }
}
