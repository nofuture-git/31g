using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Contract.US.Excuse
{
    /// <summary>
    /// <![CDATA[ 
    /// Restatement (Second) of Contracts § 265 
    /// a party's principal purpose is substantially frustrated
    /// ]]>
    /// </summary>
    public class FrustrationOfPurpose<T> : ImpracticabilityOfPerformance<T> where T : ILegalConcept
    {
        public FrustrationOfPurpose(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// The object must be so completely the basis of the contract that, as 
        /// both parties understand, without it the transaction would make little sense.
        /// </summary>
        public Predicate<ILegalPerson> IsPrincipalPurposeFrustrated { get; set; } = lp => false;

        /// <summary>
        /// The frustration must be so severe that it is not fairly to be regarded as within 
        /// the risks that he assumed under the contract.
        /// </summary>
        public Predicate<ILegalPerson> IsFrustrationSubstantial { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);
            if (offeree == null || offeror == null)
                return false;

            if (!TryGetTerms(offeror, offeree))
            {
                return false;
            }

            var ps = new[] {IsPrincipalPurposeFrustrated, IsBasicAssumptionGone, IsFrustrationSubstantial};
            return IsPerformanceDischarged(offeror, ps) || IsPerformanceDischarged(offeree, ps);
        }
    }
}
