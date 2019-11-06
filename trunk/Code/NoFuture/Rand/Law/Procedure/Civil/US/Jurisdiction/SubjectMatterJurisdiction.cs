using System;
using NoFuture.Rand.Law.US.Courts;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Different kinds of courts hear different kinds of cases
    /// </summary>
    public class SubjectMatterJurisdiction : JurisdictionBase
    {
        public SubjectMatterJurisdiction(ICourt name) : base(name)
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
        /// Congress must convey jurisdiction in a federal statute
        /// </summary>
        /// <remarks>
        /// Only some statutes require a federal court like patents and copyright.
        /// </remarks>
        public Predicate<ILegalConcept> IsCongressConveyedJurisdiction { get; set; } = lc => false;


        /// <summary>
        /// Asserts that the type of <see cref="CivilProcedureBase.Court"/>
        /// matches to the <see cref="CivilProcedureBase.CausesOfAction"/>
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCausesOfActionAssigned())
                return false;

            if (!IsCourtAssigned())
                return false;

            var isFedCourt = Court is FederalCourt;
            var isFedCourtRequired = IsArisingFromFederalLaw(CausesOfAction) &&
                                     IsCongressConveyedJurisdiction(CausesOfAction);

            //TODO - 28 U.S.C. Section 1334, exclusive federal jurisdiction (intellectual property, bankruptcy, etc.)

            if (isFedCourtRequired && !isFedCourt)
            {
                AddReasonEntry($"{nameof(IsArisingFromFederalLaw)} and {nameof(IsCongressConveyedJurisdiction)} are " +
                               $"both true for {nameof(CausesOfAction)} but the {nameof(Court)}, named '{Court?.Name}', " +
                               $"is type {Court?.GetType().Name} not type {nameof(FederalCourt)}");
                return false;
            }

            //Federal jurisdiction may also be handled by states - there is concurrent jurisdiction.
            return Court is StateCourt;
        }

    }
}
