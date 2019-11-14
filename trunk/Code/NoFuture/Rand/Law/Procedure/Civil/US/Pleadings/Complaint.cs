using System.Linq;
using NoFuture.Rand.Law.Attributes;
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
        public ILegalConcept RequestedRelief { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (persons.Any(p => p is ICourtOfficial) && !IsSignedByCourtOfficial(persons))
                return false;

            if (CausesOfAction == null)
            {
                AddReasonEntry($"{nameof(CausesOfAction)} is unassigned");
                return false;
            }

            if (RequestedRelief == null)
            {
                AddReasonEntry($"{nameof(RequestedRelief)} is unassigned");
                return false;
            }

            var result = CausesOfAction.IsValid(persons) && RequestedRelief.IsValid(persons);

            AddReasonEntryRange(CausesOfAction.GetReasonEntries());
            AddReasonEntryRange(RequestedRelief.GetReasonEntries());

            return result;
        }
    }
}
