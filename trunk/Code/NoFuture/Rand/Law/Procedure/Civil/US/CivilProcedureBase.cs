using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

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
        /// The reason the <see cref="ILegalPerson"/> is going to court in the first place.
        /// </summary>
        [Aka("subject matter")]
        public Func<ILegalPerson, ILegalConcept> GetCausesOfAction { get; set; } = lp => null;

        protected bool IsCourtAssigned()
        {
            if (Court != null && !string.IsNullOrWhiteSpace(Court.Name))
                return true;

            AddReasonEntry($"{nameof(Court)} is unassigned or an empty string");
            return false;
        }

        protected internal virtual bool TryGetCauseOfAction(ILegalPerson legalPerson, out ILegalConcept causeOfAction)
        {
            causeOfAction = null;

            if (legalPerson == null)
                return false;

            causeOfAction = GetCausesOfAction(legalPerson);

            var title = legalPerson.GetLegalPersonTypeName();

            if (causeOfAction == null)
            {
                AddReasonEntry($"{title} {legalPerson.Name}, {nameof(GetCausesOfAction)} returned nothing");
                return false;
            }

            return true;
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
