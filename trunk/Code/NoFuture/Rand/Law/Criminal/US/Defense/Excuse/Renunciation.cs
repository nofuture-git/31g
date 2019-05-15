using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Inchoate;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Defense.Excuse
{
    /// <summary>
    /// an affirmative defense for Conspiracy
    /// </summary>
    public class Renunciation : InchoateDefenseBase
    {
        public Renunciation(ICrime crime) : base(crime)
        {
        }

        public Predicate<ILegalPerson> IsVoluntarily { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsCompletely { get; set; } = lp => false;

        /// <summary>
        /// The conspiracy is a plan to commit some crime which 
        /// is called the object of the conspiracy.  It is this 
        /// crime that must have been thwarted.
        /// </summary>
        public Predicate<ILegalPerson> IsResultCrimeThwarted { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!TestIsActusReusOfType(typeof(Conspiracy), typeof(Solicitation)))
                return false;

            if (!IsCompletely(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsCompletely)} is false");
                return false;
            }
            if (!IsVoluntarily(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsVoluntarily)} is false");
                return false;
            }
            if (!IsResultCrimeThwarted(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsResultCrimeThwarted)} is false");
                return false;
            }

            return true;
        }
    }
}
