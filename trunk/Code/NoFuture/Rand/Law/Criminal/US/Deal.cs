using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.US
{
    public class Deal : LegalConcept, IAssent
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;

            if (!IsApprovalExpressed(defendant))
            {
                AddReasonEntry($"defendant {defendant.Name}, {nameof(IsApprovalExpressed)} is false");
                return false;
            }

            return true;
        }

        public Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp => false;
    }
}
