using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Donative Promise")]
    public class SocialContract : Promise
    {
        public SocialContract()
        {
            _audit.Add($"A {nameof(SocialContract)} is a donative promise and cannot be breached.");
        }

        public override bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            return false;
        }

        private readonly List<string> _audit = new List<string>();
        public override List<string> Audit => _audit;
        public override bool IsEnforceableInCourt => false;
    }
}
