using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Different kinds of courts hear different kinds of cases
    /// </summary>
    /// <remarks>
    /// When the subject matter jurisdiction is very broad then it is called &quot;general jurisdiction&quot;
    /// </remarks>
    public class FederalSubjectMatterJurisdiction : SubjectMatterJurisdiction, IFederalJurisdiction
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
        /// Some subjects are exclusively for federal courts including intellectual property, bankruptcy, etc.
        /// </summary>
        public virtual Predicate<ILegalConcept> IsExclusiveFederalJurisdiction { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (!IsFederalCourt())
                return false;

            return IsValidAsFederalCourt(persons);
        }

        public virtual bool IsValidAsFederalCourt(ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;
            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();

            if (!TryGetCauseOfAction(plaintiff, out var causesOfAction))
                return false;

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
