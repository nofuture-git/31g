using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <inheritdoc cref="IImproperThreat"/>
    public interface IImproperThreatByOne : IImproperThreat
    {
        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(1)(a)]]>
        /// </summary>
        Predicate<ILegalPerson> IsCrimeOrTort { get; set; }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(1)(b)]]>
        /// </summary>
        Predicate<ILegalPerson> IsProsecutionAsCriminal { get; set; }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(1)(c)]]>
        /// </summary>
        Predicate<ILegalPerson> IsUseCivilProcessInBadFaith { get; set; }

        /// <summary>
        /// <![CDATA[Restatement (Second) of Contracts § 176(1)(d)]]>
        /// </summary>
        Predicate<ILegalPerson> IsBreachOfGoodFaithDuty { get; set; }
    }
}