using System;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    /// <summary>
    /// This can be used as the action part of actus reus
    /// </summary>
    public class DutyToAct : ObjectiveLegalConcept, IElement
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
        /// The duty to act orginates from any form of special relationship (e.g. spouse-spouse, parent-child, etc.)
        /// </summary>
        public Predicate<ILegalPerson> IsSpecialRelationshipOrigin { get; set; } = lp => false;

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
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
