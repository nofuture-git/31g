using System;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <inheritdoc />
    /// <summary>
    /// This is a substitute for Consideration
    /// </summary>
    public class PromissoryEstoppel<T> : Consideration<T> where T : ILegalConcept
    {
        public PromissoryEstoppel(ComLawContract<T> contract) : base(contract)
        {
        }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeree = Contract.GetOfferee(persons);

            var dependent = IsOffereeDependedOnPromise ?? (o => true);
            var worse = IsOffereePositionWorse ?? (o => true);

            var isdependent = dependent(offeree);
            if (!isdependent)
            {
                AddReasonEntry($"{nameof(PromissoryEstoppel<T>)} is not a " +
                              $"consideration substitute: {nameof(IsOffereeDependedOnPromise)} " +
                              $"for {offeree?.Name} is false");
            }
            var isWorse = worse(offeree);
            if (!isWorse)
            {
                AddReasonEntry($"{nameof(PromissoryEstoppel<T>)} is not a " +
                              $"consideration substitute: {nameof(IsOffereePositionWorse)} " +
                              $"for {offeree?.Name} is false");
            }

            return isdependent && isWorse;
        }

        /// <summary>
        /// Offeree changed conduct based on promise and is now depend upon it.
        /// </summary>
        public virtual Predicate<ILegalPerson> IsOffereeDependedOnPromise { get; set; } = o => true;

        /// <summary>
        /// Retracting the gift now will leave offeree in position worse than if
        /// the promise had never been made.
        /// </summary>
        public virtual Predicate<ILegalPerson> IsOffereePositionWorse { get; set; } = o => true;
    }
}
