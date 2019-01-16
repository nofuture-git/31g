using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// false statement with intent to mislead and damages result
    /// 
    /// ]]>
    /// </summary>
    [Aka("misrepresentation")]
    public class ByFraud<T> : DefenseBase<T>, IVoidable
    {
        public ByFraud(IContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }

    }
}
