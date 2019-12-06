using System;

namespace NoFuture.Rand.Law.Procedure.Criminal.US
{
    /// <summary>
    /// Any kind of mobile vehicle is assumed to have a lower
    /// expectation of privacy and are much more open to searches
    /// </summary>
    /// <remarks>
    /// This is applicable to all kinds of vehicles including airplanes, boats and motor-homes 
    /// </remarks>
    public interface IMobileVehicleSearch : ISearch
    {
        /// <summary>
        /// Police may search an mobile vehicle and any containers within it when they
        /// have probable cause to believe that contraband or evidence of crime is present anywhere inside
        /// </summary>
        Predicate<ILegalPerson> IsBeliefEvidenceToCrimeIsPresent { get; set; }
    }
}
