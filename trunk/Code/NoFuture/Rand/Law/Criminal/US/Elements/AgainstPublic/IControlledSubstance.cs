using NoFuture.Law.Criminal.US.Terms;

namespace NoFuture.Law.Criminal.US.Elements.AgainstPublic
{
    public interface IControlledSubstance : IActusReus
    {
        IDrugSchedule Offer { get; set; }
    }
}
