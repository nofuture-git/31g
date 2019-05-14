using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// protects defendant from criminal responsibility when the defendant commits a crime to avoid a greater, imminent harm
    /// </summary>
    [Aka("choice of evils defense")]
    public interface INecessity : ILegalConcept
    {
        /// <summary>
        /// (1) there must be more than one harm that will occur under the circumstances
        /// </summary>
        Predicate<ILegalPerson> IsMultipleInHarm { get; set; }

        /// <summary>
        /// (2) the harms must be ranked, with one of the harms ranked more severe than the other
        /// </summary>
        ChoiceThereof<ITermCategory> Proportionality { get; set; }

        /// <summary>
        /// (3) the defendant must have objectively reasonable belief that the greater harm is imminent
        /// </summary>
        Imminence Imminence { get; set; }

        /// <summary>
        /// (4) [optional] the defendant did not intentionally or recklessly place himself in a
        ///     situation in which it would be probable that he would be forced to
        ///     choose the criminal conduct
        /// </summary>
        Predicate<ILegalPerson> IsResponsibleForSituationArise { get; set; }
    }
}