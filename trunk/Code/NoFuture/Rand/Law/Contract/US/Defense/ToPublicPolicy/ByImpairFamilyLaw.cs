using System;

namespace NoFuture.Law.Contract.US.Defense.ToPublicPolicy
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
            if (!base.IsValid(persons))
                return false;

            throw new NotImplementedException();
        }
    }
}
