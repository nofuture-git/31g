using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[
    /// threat of force or other unlawful action to induce to consent
    /// ]]>
    /// </summary>
    [Aka("offer (s)he couldn't refuse")]
    public class ByDuress<T> : DefenseBase<T>, IVoidable
    {
        public ByDuress(IContract<T> contract) : base(contract)
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
