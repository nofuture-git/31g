using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Defense.Insanity
{
    /// <summary>
    /// A combination of <see cref="MNaghten"/> and <see cref="IrresistibleImpulse"/> 
    /// where each predicate is scaled down to only be substantial capacity instead of 
    /// total capacity see (Model Penal Code § 4.01(1)) 
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// was in greater use but after John Hinckley's assassination attempt 
    /// of President Reagan - states and fed went back to M'Naghten.
    /// ]]>
    /// </remarks>
    [Aka("ALI defense")]
    public interface ISubstantialCapacity : ILegalConcept
    {
        /// <summary>
        /// Must lack substantial, not total, capacity to know the difference between right and wrong.
        /// <![CDATA[
        /// When appreciate is the standard, must analyze the defendant’s emotional state, 
        /// character or personality as relevant and admissible.
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsMostlyWrongnessOfAware { get; set; }

        /// <summary>
        /// Must lack substantial, not total, ability to conform conduct to the requirements of the law.
        /// </summary>
        Predicate<ILegalPerson> IsMostlyVolitional { get; set; }

        /// <summary>
        /// cognitively impaired to the level of not knowing the nature and 
        /// quality of the criminal act or that the act is wrong
        /// </summary>
        Predicate<ILegalPerson> IsMentalDefect { get; set; }
    }
}