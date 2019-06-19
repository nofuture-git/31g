using System;

namespace NoFuture.Rand.Law.Criminal
{
    /// <summary>
    /// Unlawful application of violent force
    /// </summary>
    public interface IBattery : ILegalConcept
    {
        Predicate<ILegalPerson> IsByViolence { get; set; }
    }
}
