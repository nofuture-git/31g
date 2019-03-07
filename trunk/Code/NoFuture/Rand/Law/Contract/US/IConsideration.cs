using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.Contract.US
{
    /// <summary>
    /// A general form for the idea of some this-for-that exchange 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Aka("quid pro quo", "this for that")]
    public interface IConsideration<T> : ILegalConcept
    {
        Func<ILegalPerson, T, bool> IsSoughtByPromisor { get; set; }
        Func<ILegalPerson, ILegalConcept, bool> IsGivenByPromisee { get; set; }
        Predicate<ILegalConcept> IsValueInEyesOfLaw { get; set; }
    }
}
