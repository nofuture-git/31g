using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// <![CDATA[
    /// false statement with intent to mislead and damages result
    /// ]]>
    /// </summary>
    /// <remarks>
    /// <![CDATA[Restatement (Second) of Contracts § 164]]>
    /// </remarks>
    [Aka("misrepresentation")]
    public interface IFraud : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[ (1) that there was a misrepresentation ]]>
        /// <![CDATA[ (2) that the misrepresentation was either fraudulent or material ]]> 
        /// <see cref="Misrepresentation"/>
        /// </summary>
        IMisrepresentation Misrepresentation { get; set; }

        /// <summary>
        /// <![CDATA[(3) that the misrepresentation induced the recipient to enter into the contract]]>
        /// </summary>
        Predicate<ILegalPerson> IsRecipientInduced { get; set; }

        /// <summary>
        /// <![CDATA[(4) that the recipient’s reliance on the misrepresentation was reasonable]]>
        /// </summary>
        Predicate<ILegalPerson> IsRecipientRelianceReasonable { get; set; }
    }
}