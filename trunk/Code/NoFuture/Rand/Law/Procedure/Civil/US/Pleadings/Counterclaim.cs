using System;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// Complaint in opposition to some other plaintiff&apos;s
    /// complaint both of which concern the same set of facts\circumstances.
    /// </summary>
    public class Counterclaim : Complaint
    {
        /// <summary>
        /// The cause-of-action which makes this a counter-claim instead of just a stand-alone <see cref="Complaint"/>.
        /// </summary>
        public ILegalConcept CounterCausesOfAction { get; set; }

        /// <summary>
        /// Counter claim must concern the same transaction or occurrence
        /// </summary>
        public Func<ILegalConcept, ILegalConcept, bool> IsSameTransactionOrOccurrence { get; set; } = (lc0, lc1) => false;

        /// <summary>
        /// Counter claim must concern the same facts and\or questions of law
        /// </summary>
        public Func<ILegalConcept, ILegalConcept, bool> IsSameQuestionOfLawOrFact { get; set; } = (lc0, lc1) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCausesOfActionAssigned())
                return false;

            if (CounterCausesOfAction == null)
            {
                AddReasonEntry($"{nameof(CounterCausesOfAction)} is unassigned");
                return false;
            }

            if (IsSameTransactionOrOccurrence(CounterCausesOfAction, CausesOfAction))
            {
                AddReasonEntry($"{nameof(IsSameTransactionOrOccurrence)} for " +
                               $"{nameof(CausesOfAction)} to {nameof(CounterCausesOfAction)} is false ");
                return false;
            }

            if (IsSameQuestionOfLawOrFact(CounterCausesOfAction, CausesOfAction))
            {
                AddReasonEntry($"{nameof(IsSameQuestionOfLawOrFact)} for " +
                               $"{nameof(CausesOfAction)} to {nameof(CounterCausesOfAction)} is false ");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
