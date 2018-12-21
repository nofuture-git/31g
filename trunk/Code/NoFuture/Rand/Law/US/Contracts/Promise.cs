using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Note("expected commitment will be done")]
    public abstract class Promise : IObjectiveLegalConcept
    {
        public abstract bool IsValid(ILegalPerson promisor, ILegalPerson promisee);

        public abstract bool IsEnforceableInCourt { get; }

        public abstract IList<string> Audit { get; }
    }
}
