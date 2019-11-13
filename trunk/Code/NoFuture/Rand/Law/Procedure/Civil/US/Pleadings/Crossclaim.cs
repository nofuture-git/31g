using System;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// Similar to <see cref="Counterclaim"/> except it concerns members
    /// of the same adversarial side
    /// (e.g. defendant-against-defendant, plaintiff-against-plaintiff)
    /// </summary>
    /// <remarks>
    /// Civil Procedure Rule 13(g) requires a cross claim to arise out of the same
    /// transaction or occurrence
    /// </remarks>
    /// <remarks>
    /// Civil Procedure Rule 18(a) - after there is at least one cross-claim related
    /// to the original transaction\occurrence, then, and only then, can unrelated
    /// claims targeting the co-defendant\co-plaintiff be tacked on.
    /// </remarks>
    public class Crossclaim : Complaint
    {
        /// <summary>
        /// The cause-of-action which makes this a counter\cross claim instead of just a stand-alone <see cref="Complaint"/>.
        /// </summary>
        public ILegalConcept OppositionCausesOfAction { get; set; }

        /// <summary>
        /// Counter\Cross claim must concern the same transaction or occurrence
        /// </summary>
        public Func<ILegalConcept, ILegalConcept, bool> IsSameTransactionOrOccurrence { get; set; } = (lc0, lc1) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCausesOfActionAssigned())
                return false;

            if (OppositionCausesOfAction == null)
            {
                AddReasonEntry($"{nameof(OppositionCausesOfAction)} is unassigned");
                return false;
            }

            if (IsSameTransactionOrOccurrence(OppositionCausesOfAction, CausesOfAction))
            {
                AddReasonEntry($"{nameof(IsSameTransactionOrOccurrence)} for " +
                               $"{nameof(CausesOfAction)} to {nameof(OppositionCausesOfAction)} is false ");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
