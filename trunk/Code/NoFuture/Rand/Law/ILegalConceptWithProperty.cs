namespace NoFuture.Rand.Law
{
    /// <summary>
    /// Simple interface to bind a legal concept to some form of legal property
    /// </summary>
    public interface ILegalConceptWithProperty  : ILegalConcept
    {
        ILegalProperty SubjectProperty { get; set; }
    }
}
