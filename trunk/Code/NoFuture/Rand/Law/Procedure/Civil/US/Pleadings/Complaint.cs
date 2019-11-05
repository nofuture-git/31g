using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Pleadings
{
    /// <summary>
    /// The initial step to starting a lawsuit adversarial 
    /// </summary>
    /// <remarks>
    /// FRCP Title II, Rule 3.
    /// https://en.wikipedia.org/wiki/Cause_of_action
    /// </remarks>
    [Aka("civil action", "law suit", "suit of law")]
    public class Complaint : PleadingBase
    {
        public ILegalConcept CausesOfAction { get; set; }

        public ILegalConcept RequestedRelief { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!IsCourtAssigned())
                return false;

            if (IsSignedByCourtOfficial(persons))
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
