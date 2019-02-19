namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    /// <inheritdoc />
    public abstract class StandardsBase<T> : DilemmaBase<T> where T : ILegalConcept
    {
        protected StandardsBase(IContract<T> contract) : base(contract)
        {
        }


        protected internal abstract bool StandardsTest(ILegalConcept a, ILegalConcept b);

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = Contract.GetOfferor(persons);
            var offeree = Contract.GetOfferee(persons);

            if (!TryGetTerms(offeror, offeree))
            {
                return false;
            }

            if (!TryGetOfferAcceptance(offeror, offeree))
            {
                return false;
            }

            if (!TryGetActualOfferAcceptance(offeror, offeree))
            {
                return false;
            }

            if (!StandardsTest(Offer, OfferActual))
            {
                AddReasonEntry($"the offeror, {offeror.Name}, did not perform as expected");
                return false;
            }

            if (!StandardsTest(Acceptance, AcceptanceActual))
            {
                AddReasonEntry($"the offeree, {offeree.Name}, did not perform as expected");
                return false;
            }

            return true;
        }
    }
}
