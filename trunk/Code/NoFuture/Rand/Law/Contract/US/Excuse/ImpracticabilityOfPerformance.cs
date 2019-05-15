using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US.Excuse
{
    /// <summary>
    /// <![CDATA[ 
    /// Restatement (Second) of Contracts § 261 
    /// "Impracticability" means more than "impracticality"
    /// ]]>
    /// </summary>
    public class ImpracticabilityOfPerformance<T> : ExcuseBase<T> where T : ILegalConcept
    {
        public ImpracticabilityOfPerformance(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// occurrence of an event the non-occurrence of which was a basic assumption on which the contract was made.
        /// </summary>
        /// <remarks>
        /// economic circumstances would need to be economic shocks or the like.
        /// </remarks>
        public Predicate<ILegalPerson> IsBasicAssumptionGone { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = persons.Offeror();
            var offeree = persons.Offeree();

            if (!TryGetTerms(offeror, offeree))
            {
                return false;
            }

            return IsPerformanceDischarged(offeror, IsBasicAssumptionGone) || IsPerformanceDischarged(offeree, IsBasicAssumptionGone);
        }

    }
}
