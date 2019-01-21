using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy
{
    /// <summary>
    /// contracts that restrict competition
    /// </summary>
    public class ByRestrictCompetition<T> : DefenseBase<T>, IVoidable
    {
        public ByRestrictCompetition(IContract<T> contract) : base(contract)
        {
        }
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!base.IsValid(offeror, offeree))
                return false;

            throw new NotImplementedException();
        }
    }
}
