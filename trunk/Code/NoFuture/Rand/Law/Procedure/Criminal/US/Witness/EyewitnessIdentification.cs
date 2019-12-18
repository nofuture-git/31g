using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Procedure.Criminal.US.Interrogations;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Witness
{
    /// <summary>
    /// A observer of a crime which may discriminate a suspect from a group.
    /// </summary>
    /// <remarks>
    /// Does not violate fourth amendment since its id of publicly presented
    /// physical appearance which has no expectation of privacy.
    /// </remarks>
    /// <remarks>
    /// Does not violate fifth amendment gives no evidence of testimonial significance.
    /// </remarks>
    [Aka("identification testimony", "police lineup")]
    public class EyewitnessIdentification : CommencementJudicialProceedings
    {
        public Func<ILegalPerson[], ILegalPerson> GetSuspect { get; set; } = lps => lps.Suspect();

        /// <summary>
        /// Required whenever <see cref="CommencementJudicialProceedings.IsJudicialProceedingsInitiated"/> is true
        /// </summary>
        public RightToCounselApproach RightToCounsel { get; set; }

        /// <summary>
        /// one of three required defects in the identification procedure
        /// </summary>
        public Predicate<ILegalPerson> IsProcedureSuggestive { get; set; } = lp => false;

        /// <summary>
        /// two of three required defects in the identification procedure
        /// </summary>
        public Predicate<ILegalPerson> IsProcedureUnnecessary { get; set; } = lp => false;

        /// <summary>
        /// three of three required defects in the identification procedure
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// Reliability is based on
        /// (1) witness viewed a perpetrator at time of the crime
        /// (2) witness attention
        /// (3) others description of perpetrator
        /// (4) witness certainty at confrontation
        /// (5) timespan between crime and confrontation
        /// ]]>
        /// </remarks>
        public Predicate<ILegalPerson> IsProcedureUnreliable { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var isJudicialProceedingInit = IsJudicialProceedingsInitiated(persons);

            if (isJudicialProceedingInit)
            {
                if (RightToCounsel == null)
                {
                    AddReasonEntry($"{nameof(IsJudicialProceedingsInitiated)} is true " +
                                   $"but {nameof(RightToCounsel)} is unassigned");
                }
                RightToCounsel.CurrentDateTime = CurrentDateTime;
                RightToCounsel.GetDateInitiationJudicialProceedings = GetDateInitiationJudicialProceedings;

                if (!RightToCounsel.IsValid(persons))
                {
                    AddReasonEntry($"{nameof(RightToCounsel)} {nameof(IsValid)} is false");
                    AddReasonEntryRange(RightToCounsel.GetReasonEntries());
                    return false;
                }
            }

            var suspect = this.Suspect(persons, GetSuspect);
            if (suspect == null)
                return false;

            var suspectTitle = suspect.GetLegalPersonTypeName();

            if (IsProcedureSuggestive(suspect) && IsProcedureUnnecessary(suspect) && IsProcedureUnreliable(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsProcedureSuggestive)}, " +
                               $"{nameof(IsProcedureUnnecessary)} and {nameof(IsProcedureUnreliable)} are all true");
                return false;
            }

            return true;
        }
    }
}
