using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Persons
{
    /// <summary>
    /// A third-party brought into a claim by a defendant
    /// whom the defendant claims is partially or fully
    /// responsible for the claim.
    /// </summary>
    [Aka("third-party defendant")]
    public interface IImpleader : IThirdParty, IDefendant
    {
    }
}
