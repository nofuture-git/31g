using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy
{
    [Aka("no honest man would offer and no sane man would sign")]
    public class ByUnconscionability<T> : DefenseBase<T>, IVoidable
    {
        public ByUnconscionability(IContract<T> contract) : base(contract)
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
