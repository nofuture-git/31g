using System;
using NoFuture.Rand.Law.Criminal.US;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US
{
    public class Deal : CriminalBase, IAssent
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
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
