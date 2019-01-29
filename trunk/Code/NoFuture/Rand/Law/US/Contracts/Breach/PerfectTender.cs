using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US.Contracts.Semiosis;

namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    /// <summary>
    /// <![CDATA[
    /// If a party’s tender of performance fails in any way 
    /// to meet the contract requirements, the party cannot 
    /// recover under the contract
    /// ]]>
    /// </summary>
    /// <remarks>
    /// does not apply to UCC installment contracts
    /// </remarks>
    [Aka("UCC 2-601")]
    public class PerfectTender<T> : DilemmaBase<T>
    {
        public  PerfectTender(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }

    }
}
