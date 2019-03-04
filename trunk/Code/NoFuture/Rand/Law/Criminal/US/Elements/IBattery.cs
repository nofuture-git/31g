using System;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// Unlawful application of force on another
    /// </summary>
    public interface IBattery : IElement
    {
        Predicate<ILegalPerson> IsByForce { get; set; }
    }
}
