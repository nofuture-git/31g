using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US
{
    public interface IControlledSubstance : IActusReus
    {
        IDrugSchedule Offer { get; set; }
    }
}
