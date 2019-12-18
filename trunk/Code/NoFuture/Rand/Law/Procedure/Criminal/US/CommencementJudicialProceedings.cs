using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    public abstract class CommencementJudicialProceedings : LegalConcept
    {
        public virtual DateTime? CurrentDateTime { get; set; }

        /// <summary>
        /// The point at which the suspect is &quot;accused&quot;.  At the Federal level
        /// its the point at which one is charged or appearance at arraignment or preliminary hearing
        /// </summary>
        public virtual Func<ILegalPerson[], DateTime?> GetDateInitiationJudicialProceedings { get; set; } = lps => null;

        protected internal virtual bool IsJudicialProceedingsInitiated(ILegalPerson[] persons)
        {
            var currentDt = CurrentDateTime ?? DateTime.UtcNow;

            var initDt = GetDateInitiationJudicialProceedings(persons);

            if (initDt == null)
                return false;

            var isJudicialInit = currentDt > initDt;

            if (!isJudicialInit)
            {
                AddReasonEntry($"{nameof(CurrentDateTime)} {currentDt} " +
                               $"is after {nameof(GetDateInitiationJudicialProceedings)} of {initDt}");

            }

            return isJudicialInit;
        }
    }
}
