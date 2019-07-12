namespace NoFuture.Rand.Law
{
    /// <summary>
    /// Simple interface to bind a legal concept to some form of legal property
    /// </summary>
    public interface ILegalConceptWithProperty<T>  : ILegalConcept where T : ILegalProperty
    {
        T SubjectProperty { get; set; }
    }
}
