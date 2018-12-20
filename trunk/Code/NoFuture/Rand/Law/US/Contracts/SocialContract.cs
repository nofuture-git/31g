using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Donative Promise")]
    public class SocialContract : Promise
    {
        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            throw new NotSupportedException("A donative promise is not enforceable.");
        }

        public override bool IsEnforceableInCourt => false;
    }
}
