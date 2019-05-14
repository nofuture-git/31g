using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Defense.Insanity
{
    /// <summary>
    /// similar to <see cref="MNaghten"/> only its considered simpler to prove and 
    /// is rejected in most jurisdictions
    /// </summary>
    public interface IIrresistibleImpulse : ILegalConcept
    {
        /// <summary>
        /// Idea that the defendant can not control their conduct because of the mental defect
        /// </summary>
        [Aka("free choice")]
        Predicate<ILegalPerson> IsVolitional { get; set; }

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