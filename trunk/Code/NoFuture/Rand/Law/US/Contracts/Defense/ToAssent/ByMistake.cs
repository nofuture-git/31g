using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// a mistake concerning something fundamental to the nature of the contract
    /// ]]>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ByMistake<T> : DefenseBase<T>, IVoidable
    {
        public ByMistake(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }
    }
}
