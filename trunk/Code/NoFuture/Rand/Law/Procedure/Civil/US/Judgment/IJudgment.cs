namespace NoFuture.Rand.Law.Procedure.Civil.US.Judgment
{
    public interface IJudgment : ILegalConcept
    {
        ICourt Court { get; set; }
    }
}
