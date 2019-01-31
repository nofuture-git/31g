using System;

namespace NoFuture.Rand.Law.US.Contracts.Remedy.MoneyDmg
{
    /// <inheritdoc />
    /// <summary>
    /// sum of money to compensate the plaintiff.
    /// </summary>
    public abstract class MoneyDmgBase<T> : RemedyBase<T> where T : IObjectiveLegalConcept
    {
        protected MoneyDmgBase(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// The way to take the object of the contract (either expected or actual) and turn it into a dollar amount
        /// </summary>
        public Func<IObjectiveLegalConcept, decimal> CalcValue { get; set; } = o => 0m;

        /// <summary>
        /// The mathematical convention for calculation of damages
        /// </summary>
        public Func<decimal, decimal, decimal> CalcDmg { get; set; } = (m1, m2) => m1 - m2;

        public Func<decimal, decimal> Rounding { get; set; } = Math.Round;

        public decimal Equivalent2Zero { get; set; } = 0.01m;

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!TryGetOfferAcceptance(offeror, offeree))
            {
                return false;
            }

            if (!TryGetActualOfferAcceptance(offeror, offeree))
            {
                return false;
            }

            if (CalcValue == null)
            {
                AddReasonEntry("there is no manner of calculation defined");
                return false;
            }

            var offerValue = CalcValue(Offer);

            AddReasonEntry($"{offeror.Name} {nameof(Offer)} value calculated as {offerValue}");

            var accpetanceValue = CalcValue(Acceptance);
            AddReasonEntry($"{offeree.Name} {nameof(Acceptance)} value calculated as {accpetanceValue}");

            var offerActualValue = CalcValue(OfferActual);
            AddReasonEntry($"{offeror.Name} {nameof(OfferActual)} value calculated as {offerActualValue}");

            var acceptanceActualValue = CalcValue(AcceptanceActual);
            AddReasonEntry($"{offeree.Name} {nameof(AcceptanceActual)} value calculated as {acceptanceActualValue}");

            var offerDiff = CalcDmg(offerValue, offerActualValue);
            offerDiff = Rounding(offerDiff);
            AddReasonEntry($"{nameof(CalcDmg)} between {offerValue} and {offerActualValue} is {offerDiff}");

            var acceptanceDiff = CalcDmg(accpetanceValue, acceptanceActualValue);
            acceptanceDiff = Rounding(acceptanceDiff);
            AddReasonEntry($"{nameof(CalcDmg)} between {accpetanceValue} and {acceptanceActualValue} is {acceptanceDiff}");

            var balance = CalcDmg(offerDiff, acceptanceDiff);
            balance = Rounding(balance);

            return balance <= Equivalent2Zero;

        }
    }
}
