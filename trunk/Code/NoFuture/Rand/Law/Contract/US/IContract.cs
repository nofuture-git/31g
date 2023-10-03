using NoFuture.Law.Attributes;

namespace NoFuture.Law.Contract.US
{
    /// <inheritdoc />
    [EtymologyNote("Latin", "'com' + 'trahere'", "assimilation of (with, together) + (to pull, drag)")]
    public interface IContract<T> : IBargain<T, ILegalConcept>
    {
    }
}
