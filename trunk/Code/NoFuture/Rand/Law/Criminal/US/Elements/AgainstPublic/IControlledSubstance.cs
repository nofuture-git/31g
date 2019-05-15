using NoFuture.Rand.Law.Criminal.US.Terms;

namespace NoFuture.Rand.Law.Criminal.US.Elements.AgainstPublic
{
    public interface IControlledSubstance : IActusReus
    {
        IDrugSchedule Offer { get; set; }
    }
}
