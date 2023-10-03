namespace NoFuture.Law.US.Persons
{
    /// <summary>
    /// Parties order to be added into a case by the court in
    /// order for the resulting judgment to be considered fair.
    /// </summary>
    public interface IAbsentee : IThirdParty
    {
        bool IsIndispensable { get; set; }
    }
}
