using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy
{
    /// <summary>
    /// contracts that impair family law obligations
    /// </summary>
    public class ByImpairFamilyLaw<T> : DefenseBase<T>, IVoidable
    {
        public ByImpairFamilyLaw(IContract<T> contract) : base(contract)
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
