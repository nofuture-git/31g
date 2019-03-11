using System;

namespace NoFuture.Rand.Law.Criminal.US.Elements
{
    /// <summary>
    /// General type for crimes which require two people
    /// </summary>
    public interface IBipartite
    {
        Predicate<ILegalPerson> IsOneOfTwo { get; set; }
    }
}
