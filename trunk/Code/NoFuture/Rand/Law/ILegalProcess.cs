namespace NoFuture.Rand.Law
{
    /// <summary>
    /// Type for the sequential processing of some legal procedure
    /// </summary>
    public interface ILegalProcess : IRationale
    {
        /// <summary>
        /// The means of moving the procedure forward from one state to another
        /// </summary>
        ILegalConcept PerformAction(ILegalConcept action, params ILegalPerson[] persons);
    }
}
