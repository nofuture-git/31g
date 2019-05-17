using System;

namespace NoFuture.Rand.Law.US
{
    /// <summary>
    /// This can be used as the action part of actus reus
    /// </summary>
    public class Duty : LegalConcept
    {
        /// <summary>
        /// The duty to act originates from a statute
        /// </summary>
        public Predicate<ILegalPerson> IsStatuteOrigin { get; set; } = lp => false;

        /// <summary>
        /// The duty to act originates from a contract
        /// </summary>
        public Predicate<ILegalPerson> IsContractOrigin { get; set; } = lp => false;

        /// <summary>
        /// The duty to act originates from any form of special relationship (e.g. spouse-spouse, parent-child, etc.)
        /// </summary>
        public Predicate<ILegalPerson> IsSpecialRelationshipOrigin { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (IsStatuteOrigin(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name} did not act according to statute");
                return true;
            }

            if (IsContractOrigin(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name} did not act according to contract");
                return true;
            }

            if (IsSpecialRelationshipOrigin(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name} did not act according to special relationship");
                return true;
            }

            return false;
        }
    }
}
