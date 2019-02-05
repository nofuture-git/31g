﻿using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <inheritdoc />
    /// <summary>
    /// A binding of a person to a expected performance
    /// </summary>
    [Note("expected commitment will be done")]
    public abstract class Promise : ObjectiveLegalConcept
    {
        public override bool IsEnforceableInCourt => true;
    }
}
