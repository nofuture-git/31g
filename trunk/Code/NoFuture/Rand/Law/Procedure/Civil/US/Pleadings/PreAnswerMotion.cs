using System;
using NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// An optional, alternative to answering a complaint 
    /// </summary>
    public class PreAnswerMotion : Complaint
    {
        /// <summary>
        /// Dismiss a complaint because the jurisdiction is invalid
        /// </summary>
        public IJurisdiction Jurisdiction { get; set; }

        /// <summary>
        /// Dismiss &quot;for failure to state a claim upon which relief can be granted&quot;
        /// </summary>
        public Predicate<ILegalConcept> IsReliefCanBeGranted { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (Jurisdiction != null && !Jurisdiction.IsValid(persons))
            {
                AddReasonEntryRange(Jurisdiction.GetReasonEntries());
                return true;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();

            if (!TryGetRequestedRelief(plaintiff, out var requestedRelief))
                return false;

            if (!IsReliefCanBeGranted(requestedRelief))
            {
                AddReasonEntry($"{plaintiffTitle} {plaintiff.Name}, {nameof(IsReliefCanBeGranted)} is false");
                return true;
            }

            return false;
        }
    }
}
