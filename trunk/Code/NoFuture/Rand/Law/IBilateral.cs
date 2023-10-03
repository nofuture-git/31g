using System;

namespace NoFuture.Law
{
    /// <summary>
    /// General type for crimes which require two people
    /// </summary>
    public interface IBilateral : ILegalConcept
    {
        Predicate<ILegalPerson> IsOneOfTwo { get; set; }
    }
}
