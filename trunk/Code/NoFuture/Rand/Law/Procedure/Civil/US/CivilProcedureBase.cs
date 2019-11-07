using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Procedure.Civil.US
{
    /// <summary>
    /// Base type for all US civil procedure types
    /// </summary>
    public abstract class CivilProcedureBase : LegalConcept
    {
        /// <summary>
        /// The Court in which the procedure is being conducted
        /// </summary>
        public ICourt Court { get; set; }

        /// <summary>
        /// The basis on which the procedure is being
        /// performed - the reason to go to court in the first place.
        /// </summary>
        [Aka("subject matter")]
        public ILegalConcept CausesOfAction { get; set; }

        protected bool IsCourtAssigned()
        {
            if (Court != null && !string.IsNullOrWhiteSpace(Court.Name))
                return true;

            AddReasonEntry($"{nameof(Court)} is unassigned or an empty string");
            return false;
        }

        protected bool IsCausesOfActionAssigned()
        {
            if (CausesOfAction != null) 
                return true;
            AddReasonEntry($"{nameof(CausesOfAction)} is unassigned");
            return false;
        }

        /// <summary>
        /// Allows for class level overrides -default is the static VocaBase.Equals
        /// </summary>
        public virtual bool NamesEqual(IVoca voca1, IVoca voca2)
        {
            return VocaBase.Equals(voca1, voca2);
        }
    }
}
