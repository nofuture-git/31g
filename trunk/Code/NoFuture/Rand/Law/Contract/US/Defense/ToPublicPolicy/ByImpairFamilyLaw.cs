using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US.Defense.ToPublicPolicy
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
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!base.IsValid(offeror, offeree))
                return false;

            throw new NotImplementedException();
        }
    }
}
