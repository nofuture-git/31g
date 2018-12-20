using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Note("expected commitment will be done")]
    public abstract class Promise
    {
        public abstract bool IsValid(ILegalPerson promisor, ILegalPerson promisee);

        public abstract bool IsEnforceableInCourt { get; }
    }
}
