using System.Collections.Generic;

namespace NoFuture.Rand.Law.US
{
    public abstract class LegalDuty : IObjectiveLegalConcept
    {
        public abstract bool IsValid(ILegalPerson promisor, ILegalPerson promisee);

        public abstract bool IsEnforceableInCourt { get; }

        public abstract IList<string> Audit { get; }
    }
}
