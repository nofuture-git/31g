using System;

namespace NoFuture.Rand.Law.Criminal.HominiLupus.US
{
    /// <summary>
    /// acts which are not themselves illegal - typical friction of the world
    /// </summary>
    public interface IAgitate : ILegalConcept
    {
        /// <summary>
        /// Being the apprehension for his or her safety or the safety of others
        /// </summary>
        Predicate<ILegalPerson> IsCauseToFearSafety { get; set; }
        Predicate<ILegalPerson> IsSubstantialEmotionalDistress { get; set; }
    }
}
