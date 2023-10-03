using System;
using NoFuture.Law.Criminal.US.Elements.Act;
using NoFuture.Law.Criminal.US.Elements.Intent.ComLaw;
using NoFuture.Law.Criminal.US.Elements.Intent.PenalCode;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US.Elements.AgainstGov
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
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var procured = IsKnowinglyProcured(defendant);
            var received = IsKnowinglyReceived(defendant);

            if (!procured && !received)
            {
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(IsKnowinglyProcured)} is false");
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(IsKnowinglyReceived)} is false");
                return false;
            }

            if (!IsPublicOfficial(defendant))
            {
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(IsPublicOfficial)} is false");
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
