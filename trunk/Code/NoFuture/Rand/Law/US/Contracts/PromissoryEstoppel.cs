using System;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <summary>
    /// This is a substitute for Consideration
    /// </summary>
    public class PromissoryEstoppel : Consideration<DonativePromise>
    {
        public PromissoryEstoppel(LegalContract<DonativePromise> contract) : base(contract)
        {
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            var dependent = IsOffereeDependedOnPromise ?? (o => true);
            var worse = IsOffereePositionWorse ?? (o => true);

            return dependent(offeree) && worse(offeree);
        }

        /// <summary>
        /// Offeree changed conduct based on promise and is now depend upon it.
        /// </summary>
        public Predicate<ILegalPerson> IsOffereeDependedOnPromise { get; set; } = o => true;

        /// <summary>
        /// Retracting the gift now will leave offeree in position worse than if
        /// the promise had never been made.
        /// </summary>
        public Predicate<ILegalPerson> IsOffereePositionWorse { get; set; } = o => true;
    }
}
