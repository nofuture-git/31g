namespace NoFuture.Rand.Law.US.Persons
{
    /// <summary>
    /// Persons who supply confidential information to the police
    /// </summary>
    public interface IInformant : ILegalPerson
    {
        bool IsPersonSufficientlyCredible { get; set; }
        bool IsInformationSufficientlyCredible { get; set; }
    }
}
