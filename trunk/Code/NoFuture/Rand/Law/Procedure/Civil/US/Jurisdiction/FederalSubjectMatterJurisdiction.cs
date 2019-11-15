using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Courts;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Different kinds of courts hear different kinds of cases
    /// </summary>
    /// <remarks>
    /// When the subject matter jurisdiction is very broad then it is called &quot;general jurisdiction&quot;
    /// </remarks>
    public class FederalSubjectMatterJurisdiction : FederalJurisdictionBase
    {
        public FederalSubjectMatterJurisdiction(ICourt name) : base(name)
        {
        }

        /// <summary>
        /// When the plaintiff, in order to establish a claim relies on federal law.
        /// </summary>
        /// <remarks>
        /// &quot;Well-Pleaded Complaint rule&quot;
        /// Only the plaintiff&apos;s actual claim must depend on federal
        /// law, not the expected defense nor possible plaintiff claims.
        /// <![CDATA[Louisville & Nashville R.R. v. Mottley, 211 U.S. 149 (1908) ]]>
        /// </remarks>
        public Predicate<ILegalConcept> IsArisingFromFederalLaw { get; set; } = lc => false;

        /// <summary>
        /// the Legislature must still authorize the court to exercise jurisdiction
        /// </summary>
        /// <remarks>
        /// Where Due-Process clause defines the outer bounds of permissible jurisdictional
        /// power - the legislature may limit but never expand jurisdiction beyond it.
        /// </remarks>
        public virtual Predicate<ILegalConcept> IsAuthorized2ExerciseJurisdiction { get; set; } = lp => true;

        /// <summary>
        /// Some subjects are exclusively for federal courts including intellectual property, bankruptcy, etc.
        /// </summary>
        public virtual Predicate<ILegalConcept> IsExclusiveFederalJurisdiction { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (!IsFederalCourt())
                return false;

            return IsValidWithoutTestCourtType(persons);
        }

        protected internal override bool IsValidWithoutTestCourtType(ILegalPerson[] persons)
        {
            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;
            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();

            if (!TryGetCauseOfAction(plaintiff, out var causesOfAction))
                return false;

            if (!IsAuthorized2ExerciseJurisdiction(causesOfAction))
            {
                AddReasonEntry($"{nameof(Court)} '{Court?.Name}' and {nameof(GetCausesOfAction)} from " +
                               $"{plaintiffTitle} {plaintiff.Name}, {nameof(IsAuthorized2ExerciseJurisdiction)} is false");
                return false;
            }

            var isAriseFedLaw = IsArisingFromFederalLaw(causesOfAction);
            var isExclusiveFed = IsExclusiveFederalJurisdiction(causesOfAction);

            if (!isAriseFedLaw && !isExclusiveFed)
            {
                AddReasonEntry($"{nameof(GetCausesOfAction)} from {plaintiffTitle} {plaintiff.Name}, " +
                               $"{nameof(IsArisingFromFederalLaw)} is false");
                AddReasonEntry($"{nameof(GetCausesOfAction)} from {plaintiffTitle} {plaintiff.Name}, " +
                               $"{nameof(IsExclusiveFederalJurisdiction)} is false");
                return false;
            }

            //Federal jurisdiction may also be handled by states - there is concurrent jurisdiction.
            return true;
        }

    }
}
