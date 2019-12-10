using System;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Searches
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

        /// <summary>
        /// with consent law enforcement may search without any justification
        /// to whatever scope the consenting person gives.
        /// </summary>
        /// <remarks>
        /// When a 3rd party has apparent or actual authority then they may consent at the lose of the absentee
        /// </remarks>
        IConsent Consent { get; set; }
    }
}
