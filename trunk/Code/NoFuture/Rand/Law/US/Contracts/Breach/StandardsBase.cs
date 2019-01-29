using System;

namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    /// <inheritdoc />
    public abstract class StandardsBase<T> : DilemmaBase<T> where T : IObjectiveLegalConcept
    {
        protected StandardsBase(IContract<T> contract) : base(contract)
        {
        }
        /// <summary>
        /// Function to get the results of a given party
        /// </summary>
        public Func<ILegalPerson, T> ActualPerformance { get; set; } = lp => default(T);

        protected internal abstract bool StandardsTest(IObjectiveLegalConcept a, IObjectiveLegalConcept b);

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!TryGetTerms(offeror, offeree))
            {
                return false;
            }

            var rorPromised = Contract.Offer;
            var reePromised = Contract.Acceptance(rorPromised);
            if (rorPromised == null)
            {
                AddReasonEntry($"there is no offer from {offeror.Name}");
                return false;
            }

            if (reePromised == null)
            {
                AddReasonEntry($"there is no return promise or performance given by {offeree.Name}");
                return false;
            }

            var rorActual = ActualPerformance(offeror);
            if (rorActual == null)
            {
                AddReasonEntry($"the offeror, {offeror.Name}, did not perform anything");
                return false;
            }

            var reeActual = ActualPerformance(offeree);
            if (reeActual == null)
            {
                AddReasonEntry($"the offeree, {offeree.Name}, did not perform anything");
            }

            if (!StandardsTest(rorPromised, rorActual))
            {
                AddReasonEntry($"the offeror, {offeror.Name}, did not perform as expected");
                return false;
            }

            if (!StandardsTest(reePromised, reeActual))
            {
                AddReasonEntry($"the offeree, {offeree.Name}, did not perform as expected");
            }

            return true;
        }
    }
}
