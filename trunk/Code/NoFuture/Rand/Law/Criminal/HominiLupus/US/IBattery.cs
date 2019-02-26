using System;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US
{
    /// <summary>
    /// Unlawful application of force on another
    /// </summary>
    public interface IBattery : ILegalConcept
    {
        Predicate<ILegalPerson> IsByForce { get; set; }
    }
}
