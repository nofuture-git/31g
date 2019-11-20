using System;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    public interface IJurisdiction : ILegalConcept
    {
        ICourt Court { get; set; }
    }
}