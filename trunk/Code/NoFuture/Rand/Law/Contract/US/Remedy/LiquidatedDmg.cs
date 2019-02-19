using System;

namespace NoFuture.Rand.Law.Contract.US.Remedy
{
    /// <inheritdoc />
    /// <summary>
    /// damages that the parties have agreed in advance that they would pay if the contract were breached
    /// </summary>
    /// <remarks>
    /// liquidate in law means transforming an unquantified legal right into a specific sum of money
    /// </remarks>
    public class LiquidatedDmg<T> : RemedyBase<T> where T : ILegalConcept
    {
        public LiquidatedDmg(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// the payment of a sum of money grossly disproportionate to the 
        /// amount of actual damages provides for penalty and is unenforceable
        /// </summary>
        public Predicate<ILegalPerson> IsDisproportionateToActual { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = Contract.GetOfferor(persons);
            var offeree = Contract.GetOfferee(persons);

            if (IsDisproportionateToActual(offeror) || IsDisproportionateToActual(offeree))
            {
                AddReasonEntry("liquidated damage clauses that are intended as a penalty are not enforceable");
                return false;
            }

            return true;
        }
    }
}
