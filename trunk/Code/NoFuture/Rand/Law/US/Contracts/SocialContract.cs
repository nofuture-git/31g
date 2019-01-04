using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Donative Promise")]
    public class SocialContract : Promise
    {
        public SocialContract()
        {
            base.AddAuditEntry($"A {nameof(SocialContract)} is a donative promise and cannot be breached.");
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return false;
        }

        public override bool IsEnforceableInCourt => false;
    }
}
