using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <inheritdoc cref="IImproperThreat"/>
    public interface IImproperThreatByTwo : IImproperThreat
    {
        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(2)]]>
        /// </summary>
        Predicate<ILegalPerson> IsUnfairTerms { get; set; }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(2)(a)]]>
        /// </summary>
        Predicate<ILegalPerson> IsAllHarmNoBenefit { get; set; }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(2)(b)]]>
        /// </summary>
        Predicate<ILegalPerson> IsSignificantViaPriorUnfairDeal { get; set; }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(2)(c)]]>
        /// </summary>
        Predicate<ILegalPerson> IsUsePowerIllegitimateEnds { get; set; }
    }
}