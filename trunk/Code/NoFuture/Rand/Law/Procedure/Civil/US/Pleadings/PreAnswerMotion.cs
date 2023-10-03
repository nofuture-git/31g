using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.Procedure.Civil.US.Jurisdiction;
using NoFuture.Law.Procedure.Civil.US.ServiceOfProcess;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// An optional, alternative to answering a complaint 
    /// </summary>
    [Aka("motion to dismiss")]
    public class PreAnswerMotion : Answer
    {
        /// <summary>
        /// (1) &amp; (3) of four procedural objections which are waived if not raised in first response to complaint
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="SpecialAppearance"/>, but these rules are taken
        /// from Fed. R. Civ. P. Rule 12, which eliminates the need to isolate personal
        /// jurisdiction exclusively. 
        /// </remarks>
        public IJurisdiction Jurisdiction { get; set; }

        /// <summary>
        /// (2) of four procedural objections which are waived if not raised in first response to complaint
        /// </summary>
        public IVenue Venue { get; set; }

        /// <summary>
        /// (4) of four procedural objections which are waived if not raised in first response to complaint
        /// </summary>
        public IProcessService ServiceOfProcess { get; set; }

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

            if (Venue != null && !Venue.IsValid(persons))
            {
                AddReasonEntryRange(Venue.GetReasonEntries());
                return true;
            }

            if (ServiceOfProcess != null && !ServiceOfProcess.IsValid(persons))
            {
                AddReasonEntryRange(ServiceOfProcess.GetReasonEntries());
                return true;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();

            var complaint = LinkedTo as Complaint;

            if (complaint == null)
            {
                AddReasonEntry($"{nameof(LinkedTo)} is not of type {nameof(Complaint)}");
                return false;
            }

            if (!complaint.TryGetRequestedRelief(plaintiff, out var requestedRelief))
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
