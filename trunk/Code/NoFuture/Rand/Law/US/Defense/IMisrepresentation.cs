using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// <![CDATA[an assertion that is not in accordance with the facts]]>
    /// </summary>
    /// <remarks>
    /// <![CDATA[ src: ALABI v. DHL AIRWAYS, INC. Superior Court of Delaware, New Castle 583 A.2d 1358 (Del. Super. 1990)]]>
    /// </remarks>
    public interface IMisrepresentation : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[
        /// (1) A misrepresentation is fraudulent if the maker intends his assertion to 
        /// induce a party to manifest his assent and the maker (a), (b) or (c)
        /// 
        /// (2) A misrepresentation is material if it would be likely to induce a 
        /// reasonable person to manifest his assent, or if the maker knows that it 
        /// would be likely to induce the recipient to do so
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsAssertionToInduceAssent { get; set; }

        /// <summary>
        /// <![CDATA[(a) knows or believes that the assertion is not in accord with the facts]]>
        /// </summary>
        Predicate<ILegalPerson> IsNotAssertionInAccord2Facts { get; set; }

        /// <summary>
        /// <![CDATA[(b) does not have the confidence that he states or implies in the truth of the assertion]]>
        /// </summary>
        Predicate<ILegalPerson> IsNotAssertionInConfidence2Truth { get; set; }

        /// <summary>
        /// <![CDATA[(c) knows that he does not have the basis that he states or implies for the assertion]]>
        /// </summary>
        Predicate<ILegalPerson> IsNotAssertionInBasis2ImpliedStatement { get; set; }

        bool IsFraudulent(ILegalPerson lp);

        bool IsMaterial(ILegalPerson lp);
    }
}