using System;
using System.Linq;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// The initial step to starting a lawsuit adversarial 
    /// </summary>
    /// <remarks>
    /// FRCP Title II, Rule 3.
    /// </remarks>
    [Aka("civil action", "law suit", "suit of law")]
    public class Complaint : PleadingBase
    {
        public Func<ILegalPerson, ILegalConcept> GetRequestedRelief { get; set; } = lp => null;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (persons.Any(p => p is ICourtOfficial) && !IsSignedByCourtOfficial(persons))
                return false;

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            if (!TryGetCauseOfAction(plaintiff, out var causeOfAction))
                return false;

            if (!TryGetRequestedRelief(plaintiff, out var requestedRelief))
                return false;

            var result = causeOfAction.IsValid(persons) && requestedRelief.IsValid(persons);

            AddReasonEntryRange(causeOfAction.GetReasonEntries());
            AddReasonEntryRange(requestedRelief.GetReasonEntries());

            return result;
        }

        protected internal virtual bool TryGetRequestedRelief(ILegalPerson legalPerson, out ILegalConcept causeOfAction)
        {
            causeOfAction = null;

            if (legalPerson == null)
                return false;

            causeOfAction = GetRequestedRelief(legalPerson);

            var title = legalPerson.GetLegalPersonTypeName();

            if (causeOfAction == null)
            {
                AddReasonEntry($"{title} {legalPerson.Name}, {nameof(GetRequestedRelief)} returned nothing");
                return false;
            }

            return true;
        }
    }
}
