using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Contract.US
{
    [Aka("social contract")]
    public class DonativePromise : Promise
    {
        public DonativePromise()
        {
            base.AddReasonEntry($"A {nameof(DonativePromise)} cannot be breached.");
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            return true;
        }

        public override bool IsEnforceableInCourt => false;
    }
}
