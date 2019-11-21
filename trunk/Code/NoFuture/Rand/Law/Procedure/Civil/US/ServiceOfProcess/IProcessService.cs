namespace NoFuture.Rand.Law.Procedure.Civil.US.ServiceOfProcess
{
    public interface IProcessService : ILegalConcept
    {
        ICourt Court { get; set; }
    }
}