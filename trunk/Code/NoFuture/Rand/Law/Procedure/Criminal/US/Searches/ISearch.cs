using System;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Searches
{
    public interface ISearch : ILegalConcept
    {
        Func<ILegalPerson[], ILegalPerson> GetConductorOfSearch { get; set; }
    }
}
