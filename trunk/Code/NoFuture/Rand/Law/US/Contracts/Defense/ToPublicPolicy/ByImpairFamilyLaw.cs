using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy
{
    /// <summary>
    /// contracts that impair family law obligations
    /// </summary>
    public class ByImpairFamilyLaw<T> : DefenseBase<T> where T : ILegalConcept
    {
        public ByImpairFamilyLaw(IContract<T> contract) : base(contract)
        {
        }
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = Contract.GetOfferor(persons);
            var offeree = Contract.GetOfferee(persons);

            if (!base.IsValid(offeror, offeree))
                return false;

            throw new NotImplementedException();
        }
    }
}
