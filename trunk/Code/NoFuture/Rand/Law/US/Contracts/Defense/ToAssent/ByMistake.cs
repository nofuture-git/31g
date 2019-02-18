using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// a mistake concerning something fundamental to the nature of the contract
    /// ]]>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ByMistake<T> : DefenseBase<T> where T : ILegalConcept
    {
        public ByMistake(IContract<T> contract) : base(contract)
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
