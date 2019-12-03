using System;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    public interface ISearch : ILegalConcept
    {
        Func<ILegalPerson[], ILegalPerson> GetConductorOfSearch { get; set; }
    }
}
