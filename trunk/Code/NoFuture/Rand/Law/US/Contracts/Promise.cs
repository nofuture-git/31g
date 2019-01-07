using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    [Note("expected commitment will be done")]
    public abstract class Promise : ObjectiveLegalConcept
    {
        public virtual DateTime? Date { get; set; }
        public override bool IsEnforceableInCourt => true;
    }
}
