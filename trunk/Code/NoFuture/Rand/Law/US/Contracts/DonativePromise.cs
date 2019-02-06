using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Aka("Social Contract")]
    public class DonativePromise : Promise
    {
        public DonativePromise()
        {
            base.AddReasonEntry($"A {nameof(DonativePromise)} cannot be breached.");
        }

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            return true;
        }

        public override bool IsEnforceableInCourt => false;
    }
}
