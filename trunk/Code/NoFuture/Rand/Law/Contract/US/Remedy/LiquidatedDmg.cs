using System;
using NoFuture.Law.US;

namespace NoFuture.Law.Contract.US.Remedy
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
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);
            if (offeree == null || offeror == null)
                return false;

            var offerorTitle = offeror.GetLegalPersonTypeName();
            var offereeTitle = offeree.GetLegalPersonTypeName();

            if (IsDisproportionateToActual(offeror))
            {
                AddReasonEntry($"{offerorTitle} {offeror.Name}, {nameof(IsDisproportionateToActual)} is true");
                return false;
            }

            if (IsDisproportionateToActual(offeree))
            {
                AddReasonEntry($"{offereeTitle} {offeree.Name}, {nameof(IsDisproportionateToActual)} is true");
                return false;
            }

            return true;
        }
    }
}
