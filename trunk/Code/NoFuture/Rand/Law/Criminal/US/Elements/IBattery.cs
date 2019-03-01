using System;
using NoFuture.Rand.Law.Criminal.US.Elements;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US
{
    /// <summary>
    /// Unlawful application of force on another
    /// </summary>
    public interface IBattery : IElement
    {
        Predicate<ILegalPerson> IsByForce { get; set; }
    }
}
