using System;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// Unlawful application of violent force
    /// </summary>
    public interface IBattery : ILegalConcept
    {
        Predicate<ILegalPerson> IsByViolence { get; set; }
    }
}
