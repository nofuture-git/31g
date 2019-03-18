using NoFuture.Rand.Law.Criminal.AgainstPublic.US.Terms;

namespace NoFuture.Rand.Law.Criminal.AgainstPublic.US.Elements
{
    public interface IControlledSubstance : IActusReus
    {
        IDrugSchedule Offer { get; set; }
    }
}
