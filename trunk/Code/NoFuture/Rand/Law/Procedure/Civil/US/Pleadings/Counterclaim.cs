using System;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// Complaint in opposition to some other plaintiff&apos;s
    /// complaint both of which concern the same set of facts\circumstances.
    /// </summary>
    public class Counterclaim : Crossclaim
    {
        /// <summary>
        /// Counter claim must concern the same facts and\or questions of law
        /// </summary>
        public Func<ILegalConcept, ILegalConcept, bool> IsSameQuestionOfLawOrFact { get; set; } = (lc0, lc1) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCausesOfActionAssigned())
                return false;

            if (OppositionCausesOfAction == null)
            {
                AddReasonEntry($"{nameof(OppositionCausesOfAction)} is unassigned");
                return false;
            }

            if (IsSameQuestionOfLawOrFact(OppositionCausesOfAction, CausesOfAction))
            {
                AddReasonEntry($"{nameof(IsSameQuestionOfLawOrFact)} for " +
                               $"{nameof(CausesOfAction)} to {nameof(OppositionCausesOfAction)} is false ");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
