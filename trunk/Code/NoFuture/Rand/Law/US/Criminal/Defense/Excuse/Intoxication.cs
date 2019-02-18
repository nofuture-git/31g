using System;

namespace NoFuture.Rand.Law.US.Criminal.Defense.Excuse
{
    /// <summary>
    /// <![CDATA[
    /// intoxication which (a) is not self-induced ... is an affirmative defense if 
    /// by reason of such intoxication 211 Criminal Law the actor at the time of his 
    /// conduct lacks substantial capacity either to appreciate its criminality 
    /// [wrongfulness] or  to conform his conduct to the requirements of law 
    /// (Model Penal Code § 2.08 (4)).
    /// ]]>
    /// </summary>
    public class Intoxication : DefenseBase
    {
        public Intoxication(ICrime crime) : base(crime)
        {
        }

        /// <summary>
        /// <![CDATA[
        /// Involuntary intoxication could affect the defendant's ability to form 
        /// criminal intent, thus negating specific intent
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsVoluntary { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsIntoxicated { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (!IsIntoxicated(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsIntoxicated)} is false");
                return false;
            }

            if (IsVoluntary != null && IsVoluntary(defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsVoluntary)} is true");
                return false;
            }

            return true;
        }
    }
}
