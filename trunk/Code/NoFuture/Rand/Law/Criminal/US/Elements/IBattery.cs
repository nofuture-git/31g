using System;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// Unlawful application of violent force
    /// </summary>
    public interface IBattery : IElement
    {
        Predicate<ILegalPerson> IsByViolence { get; set; }
    }
}
