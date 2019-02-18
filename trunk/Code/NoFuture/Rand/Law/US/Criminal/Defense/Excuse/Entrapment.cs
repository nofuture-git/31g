using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse
{
    /// <summary>
    /// when the criminal intent originated with government 
    /// </summary>
    public class Entrapment : DefenseBase
    {
        public Entrapment(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// this may be subjective or objective depending on the jurisdiction 
        /// </summary>
        public Predicate<ILegalPerson> IsIntentOriginFromLawEnforcement { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (!IsIntentOriginFromLawEnforcement(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, mens rea is {nameof(IsIntentOriginFromLawEnforcement)}");
                return false;
            }

            return true;
        }
    }
}
