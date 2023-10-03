using NoFuture.Law.Attributes;

namespace NoFuture.Law.US.Persons
{
    /// <summary>
    /// Citizen of another nation-state other than the United States
    /// </summary>
    [Aka("alien")]
    public interface IForeigner : ILegalPerson
    {
    }
}
