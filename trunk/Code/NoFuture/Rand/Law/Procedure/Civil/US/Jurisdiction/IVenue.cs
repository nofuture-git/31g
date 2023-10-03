using NoFuture.Law.Attributes;

namespace NoFuture.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Concerns the geographic location of the court
    /// </summary>
    /// <remarks>
    /// Is a county for state court and a district\division for federal court
    /// </remarks>
    [EtymologyNote("latin", "locus delicti", "place of delict")]
    public interface IVenue : ILegalConcept
    {

    }
}
