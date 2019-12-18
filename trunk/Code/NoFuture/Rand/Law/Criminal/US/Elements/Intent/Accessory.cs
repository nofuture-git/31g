using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US.Elements.Intent
{
    /// <summary>
    /// intent from assistance given after the crime
    /// </summary>
    public class Accessory : MensRea
    {
        /// <summary>
        /// awareness that the principal committed a crime
        /// </summary>
        public Predicate<ILegalPerson> IsAwareOfCrime { get; set; } = lp => false;

        /// <summary>
        /// help or assist the principal escape or evade arrest or prosecution 
        /// for and conviction of the offense with specific intent or purposely
        /// </summary>
        public Predicate<ILegalPerson> IsAssistToEvadeProsecution { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsAwareOfCrime(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsAwareOfCrime)} is false");
                return false;
            }

            if (!IsAssistToEvadeProsecution(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsAssistToEvadeProsecution)} is false");
                return false;
            }

            return true;
        }

        public override int CompareTo(object obj)
        {
            return obj is Accessory ? 0 : 1;
        }
    }
}
