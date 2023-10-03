using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Criminal.US
{
    public class Deal : LegalConcept, IAssent
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (!IsApprovalExpressed(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsApprovalExpressed)} is false");
                return false;
            }

            return true;
        }

        public Predicate<ILegalPerson> IsApprovalExpressed { get; set; } = lp => false;
    }
}
