using System;

namespace NoFuture.Rand.Law.US.Contracts
{
    public class MutualAssent
    {
        public Predicate<ILegalPerson> IsApprovalExpressed { get; set; }
        public bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (IsApprovalExpressed == null)
                return false;

            return IsApprovalExpressed(promisor) && IsApprovalExpressed(promisee);
        }
    }
}