using System;

namespace NoFuture.Law.Procedure.Criminal.US.Searches
{
    public interface ISearch : ILegalConcept
    {
        Func<ILegalPerson[], ILegalPerson> GetConductorOfSearch { get; set; }
    }
}
