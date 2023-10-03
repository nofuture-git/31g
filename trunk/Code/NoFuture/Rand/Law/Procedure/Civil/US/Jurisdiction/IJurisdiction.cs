using System;
using NoFuture.Law;

namespace NoFuture.Law.Procedure.Civil.US.Jurisdiction
{
    public interface IJurisdiction : ILegalConcept
    {
        ICourt Court { get; set; }
    }
}