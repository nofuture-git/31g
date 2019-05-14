using System;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.Inchoate.US.Elements;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Defense;

namespace NoFuture.Rand.Law.Criminal.Inchoate.US.Defense
{
    /// <inheritdoc cref="IRenunciation"/>
    public class Renunciation : InchoateDefenseBase, IRenunciation
    {
        public Renunciation(ICrime crime) : base(crime)
        {
        }

        public Predicate<ILegalPerson> IsVoluntarily { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsCompletely { get; set; } = lp => false;

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
