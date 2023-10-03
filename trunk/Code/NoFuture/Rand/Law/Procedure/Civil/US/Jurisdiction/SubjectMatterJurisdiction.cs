using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Concerns the content of the complaint and the court&apos;s entitlement to hear it
    /// </summary>
    public class SubjectMatterJurisdiction : JurisdictionBase
    {
        public SubjectMatterJurisdiction(ICourt name) : base(name)
        {
        }

        /// <summary>
        /// the Legislature must still authorize the court to exercise jurisdiction
        /// </summary>
        /// <remarks>
        /// Where Due-Process clause defines the outer bounds of permissible jurisdictional
        /// power - the legislature may limit but never expand jurisdiction beyond it.
        /// </remarks>
        public virtual Predicate<ILegalConcept> IsAuthorized2ExerciseJurisdiction { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;
            var plaintiffTitle = plaintiff.GetLegalPersonTypeName();

            if (!TryGetCauseOfAction(plaintiff, out var causesOfAction))
                return false;

            if (!IsAuthorized2ExerciseJurisdiction(causesOfAction))
            {
                AddReasonEntry($"{nameof(Court)} '{Court?.Name}' and {nameof(GetAssertion)} from " +
                               $"{plaintiffTitle} {plaintiff.Name}, {nameof(IsAuthorized2ExerciseJurisdiction)} is false");
                return false;
            }

            return true;
        }
    }
}
