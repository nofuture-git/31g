using System;

namespace NoFuture.Rand.Law.US.Persons
{
    /// <summary>
    /// an individual who is legally entitled, by statute or by will, to inherit a decedent&apos;s estate
    /// </summary>
    public interface IHeir : ILegalPerson
    {
        Predicate<ILegalPerson> IsHeirOf { get; set; }
    }
}
