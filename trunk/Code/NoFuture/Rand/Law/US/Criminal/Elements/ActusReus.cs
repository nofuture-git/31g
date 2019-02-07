using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    /// <summary>
    /// To be a crime there must have been an action which was voluntary
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// there must always
    /// ]]>
    /// </remarks>
    [Aka("criminal intent")]
    public class ActusReus : ObjectiveLegalConcept, IElement
    {
        /// <summary>
        /// There must always be something willfully
        /// </summary>
        public Predicate<ILegalPerson> IsVoluntary { get; set; } = lp => false;

        /// <summary>
        /// There must be some outward act or failure to act 
        /// (thoughts, plans, labels, status are not actus reus).
        /// </summary>
        public Predicate<ILegalPerson> IsAction { get; set; } = lp => false;

        /// <summary>
        /// There must be some outward act or failure to act 
        /// (thoughts, plans, labels, status are not actus reus).
        /// </summary>
        public DutyToAct Omission { get; set; } = new DutyToAct();

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            var defendant = Government.GetDefendant(offeror, offeree, this);
            if (defendant == null)
                return false;

            if (!IsVoluntary(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, did not perform voluntarily");
                return false;
            }

            if (Omission != null && Omission.IsValid(offeror, offeree))
            {
                AddReasonEntryRange(Omission.GetReasonEntries());
                return true;
            }

            if (!IsAction(defendant))
            {
                AddReasonEntry($"the defendant, {defendant.Name}, has not operation of an act");
                return false;
            }

            return true;
        }
    }
}
