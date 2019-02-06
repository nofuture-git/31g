using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy
{
    /// <summary>
    /// contracts that impair family law obligations
    /// </summary>
    public class ByImpairFamilyLaw<T> : DefenseBase<T> where T : IObjectiveLegalConcept
    {
        public ByImpairFamilyLaw(IContract<T> contract) : base(contract)
        {
        }
        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (!base.IsValid(offeror, offeree))
                return false;

            throw new NotImplementedException();
        }
    }
}
