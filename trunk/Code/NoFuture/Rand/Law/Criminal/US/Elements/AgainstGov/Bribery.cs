using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstGov
{
    /// <summary>
    /// conferring, offering, agreeing to confer, or soliciting, accepting,
    /// or agreeing to accept any benefit upon a public official
    /// </summary>
    public class Bribery : LegalConcept, IPossession
    {

        public Predicate<ILegalPerson> IsPublicOfficial { get; set; } = lp => false;

        /// <summary>
        /// Public official&apos;s vote, opinion, judgment, action,
        /// decision or exercise discretion will be influenced.
        /// </summary>
        public Predicate<ILegalPerson> IsKnowinglyProcured { get; set; } = lp => false;

        /// <summary>
        /// Public official&apos;s vote, opinion, judgment, action,
        /// decision or exercise discretion will be influenced.
        /// </summary>
        public Predicate<ILegalPerson> IsKnowinglyReceived { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            var procured = IsKnowinglyProcured(defendant);
            var received = IsKnowinglyReceived(defendant);

            if (!procured && !received)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsKnowinglyProcured)} is false");
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsKnowinglyReceived)} is false");
                return false;
            }

            if (!IsPublicOfficial(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsPublicOfficial)} is false");
                return false;
            }

            return true;
        }

        public bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            var isValid = criminalIntent is Purposely || criminalIntent is SpecificIntent
                                                      || criminalIntent is Knowingly || criminalIntent is GeneralIntent;
            if (!isValid)
            {
                AddReasonEntry($"{nameof(Bribery)} requires intent of {nameof(Purposely)}, " +
                               $"{nameof(SpecificIntent)}, {nameof(Knowingly)}, {nameof(GeneralIntent)}");
                return false;
            }

            return true;
        }

    }
}
