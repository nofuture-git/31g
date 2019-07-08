using System;

namespace NoFuture.Rand.Law.US.Persons
{
    /// <summary>
    /// a relational position of one person to another through hereditary means 
    /// </summary>
    public interface IDescendant : ILegalPerson
    {
        Predicate<ILegalPerson> IsDescendantOf { get; set; }
    }
}
