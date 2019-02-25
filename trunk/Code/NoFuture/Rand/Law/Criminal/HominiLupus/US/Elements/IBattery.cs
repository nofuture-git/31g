using System;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US.Elements
{
    /// <summary>
    /// Unlawful application of force on another
    /// </summary>
    public interface IBattery : IActusReus
    {
        Predicate<ILegalPerson> IsByForce { get; set; }
    }
}
