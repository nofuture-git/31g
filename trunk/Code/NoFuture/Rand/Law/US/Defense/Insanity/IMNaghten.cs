using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Defense.Insanity
{
    /// <summary>
    /// Insanity defense named after Daniel M'Naghten from England (1843).
    /// </summary>
    [Aka("right-wrong test")]
    public interface IMNaghten : ILegalConcept
    {
        /// <summary>
        /// Having a basic level of awareness under the attendant circumstances
        /// </summary>
        Predicate<ILegalPerson> IsNatureQualityOfAware { get; set; }

        /// <summary>
        /// to assert that they did not know their act was wrong
        /// </summary>
        Predicate<ILegalPerson> IsWrongnessOfAware { get; set; }

        /// <summary>
        /// cognitively impaired to the level of not knowing the nature and 
        /// quality of the criminal act or that the act is wrong
        /// </summary>
        Predicate<ILegalPerson> IsMentalDefect { get; set; }
    }
}