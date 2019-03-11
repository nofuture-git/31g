using System;

namespace NoFuture.Rand.Law.Criminal.US
{
    /// <summary>
    /// General type for crimes which require two people
    /// </summary>
    public interface IBipartite
    {
        Predicate<ILegalPerson> IsOneOfTwo { get; set; }
    }
}
