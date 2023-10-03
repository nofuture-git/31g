using NoFuture.Law.Attributes;

namespace NoFuture.Law.US.Persons
{
    /// <summary>
    /// When a defendant becomes a plaintiff by adding in some
    /// third-party as partially or fully responsible for the
    /// original plaintiff&apos;s claim
    /// </summary>
    /// <remarks>
    /// This type could be used to categorize both
    /// cross-claim&apos;ers and counter-claim&apos;ers
    /// </remarks>
    [Aka("third-party plaintiff")]
    [EtymologyNote("latin","ambifendant", "(neologism) 'ambi' + 'fendere', both-sides + to strike")]
    public interface IImpleader : IDefendant, IPlaintiff
    {
    }
}
