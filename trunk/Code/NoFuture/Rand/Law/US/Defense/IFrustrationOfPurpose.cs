using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// <![CDATA[ 
    /// Restatement (Second) of Contracts § 265 
    /// a party's principal purpose is substantially frustrated
    /// ]]>
    /// </summary>
    public interface IFrustrationOfPurpose : ILegalConcept
    {
        /// <summary>
        /// The object must be so completely the basis of the contract that, as 
        /// both parties understand, without it the transaction would make little sense.
        /// </summary>
        Predicate<ILegalPerson> IsPrincipalPurposeFrustrated { get; set; }

        /// <summary>
        /// The frustration must be so severe that it is not fairly to be regarded as within 
        /// the risks that he assumed under the contract.
        /// </summary>
        Predicate<ILegalPerson> IsFrustrationSubstantial { get; set; }

        Predicate<ILegalPerson> IsBasicAssumptionGone { get; set; }

        /// <summary>
        /// <![CDATA[
        /// a party's performance is made impracticable without his fault
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsAtFault { get; set; }

        /// <summary>
        /// unless the language or the circumstances indicate the contrary
        /// </summary>
        Predicate<ILegalPerson> IsContraryInForm { get; set; }
    }
}