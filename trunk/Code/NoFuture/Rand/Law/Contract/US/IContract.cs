using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Contract.US
{
    /// <inheritdoc />
    [EtymologyNote("Latin", "'com' + 'trahere'", "assimilation of (with, together) + (to pull, drag)")]
    public interface IContract<T> : IBargain<T>
    {
        ILegalPerson GetOfferor(ILegalPerson[] persons);

        ILegalPerson GetOfferee(ILegalPerson[] persons);
    }
}
