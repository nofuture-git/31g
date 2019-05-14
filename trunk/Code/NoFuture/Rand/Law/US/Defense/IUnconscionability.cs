using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Defense
{
    [Aka("no honest man would offer and no sane man would sign")]
    public interface IUnconscionability : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[determined by consideration of all the circumstances surrounding the transaction]]>
        /// </summary>
        Predicate<ILegalPerson> IsAbsenceOfMeaningfulChoice { get; set; }

        /// <summary>
        /// <![CDATA[contract terms which are unreasonably favorable to the other party]]>
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// The terms are to be considered "in the light of the general commercial 
        /// background and the commercial needs of the particular trade or case."
        /// ]]>
        /// </remarks>
        Predicate<ILegalPerson> IsUnreasonablyFavorableTerms { get; set; }
    }
}