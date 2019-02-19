using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent.PenalCode;

namespace NoFuture.Rand.Law.Criminal.Inchoate.US.Elements
{
    /// <summary>
    /// a crime that has only just begun which may be defined by statute or common law
    /// </summary>
    public class Attempt: ActusReus
    {
        /// <summary>
        /// A measure of criminal effort left to be done, not a measure of what is already done.
        /// </summary>
        public Predicate<ILegalPerson> IsProximity { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[ "thing itself speaks," meaning its clear that there is not other purpose but criminal ]]>
        /// </summary>
        [Aka("unequivocality test")]
        public Predicate<ILegalPerson> IsResIpsaLoquitur { get; set; } = lp => false;

        /// <summary>
        /// a point of progress where its probable that the crime will happen unless some exogenous force intervenes
        /// </summary>
        public Predicate<ILegalPerson> IsProbableDesistance { get; set; } = lp => false;

        /// <summary>
        /// Some kind of action which corroborates a proposistion of intent - evidence of a suspicion
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// Model Penal Code examples: 
        /// * lying in wait; 
        /// * enticing victem to scene; 
        /// * investigating potential scene; 
        /// * unlawful entry; 
        /// * possessing materials specially designed for unlawful use; 
        /// * possessing, collecting, or fabricating materials to be used in crime
        /// ]]>
        /// </remarks>
        public Predicate<ILegalPerson> IsSubstantial { get; set; } = lp => false;

        /// <summary>
        /// Attempt is not applicable to reckless or negligent intent
        /// </summary>
        public override bool CompareTo(MensRea criminalIntent)
        {
            var isInvalid2Attempt = criminalIntent is Recklessly || criminalIntent is Negligently;
            if (isInvalid2Attempt)
            {
                AddReasonEntry("generally, no such thing exists as reckless or negligent attempt");
                return false;
            }

            return base.CompareTo(criminalIntent);
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            var ispx = IsProximity(defendant);
            var ispd = IsProbableDesistance(defendant);
            var isril = IsResIpsaLoquitur(defendant);
            var issub = IsSubstantial(defendant);

            var isAttempt = ispx || ispd || isril || issub;
            if (!isAttempt)
            {
                AddReasonEntry($"defendant, {defendant.Name}, is attempt false");
                return false;
            }

            return true;
        }
    }
}
