using System;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// Unlawful application of force on another
    /// </summary>
    public interface IBattery
    {
        Predicate<ILegalPerson> IsByForce { get; set; }
    }
}
