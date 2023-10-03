namespace NoFuture.Law.Criminal.US.Terms
{
    public interface IDrugSchedule : ITermCategory
    {
        bool IsAcceptedMedicalUse { get; }
    }
}
