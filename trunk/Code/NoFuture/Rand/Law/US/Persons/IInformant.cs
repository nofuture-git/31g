using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Persons
{
    /// <summary>
    /// Persons who supply confidential information to the police
    /// </summary>
    [Aka("snitch")]
    public interface IInformant : ILegalPerson
    {
        /// <summary>
        /// For example, this informant has provided reliable information in the past
        /// </summary>
        bool IsPersonSufficientlyCredible { get; set; }

        /// <summary>
        /// When the information is specific to some facts or circumstances
        /// </summary>
        bool IsInformationSufficientlyReliable { get; set; }
    }
}
