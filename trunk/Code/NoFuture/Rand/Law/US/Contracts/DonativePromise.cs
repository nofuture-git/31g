using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Social Contract")]
    public class DonativePromise : Promise
    {
        public DonativePromise()
        {
            base.AddAuditEntry($"A {nameof(DonativePromise)} cannot be breached.");
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return true;
        }

        public override bool IsEnforceableInCourt => false;
    }
}
