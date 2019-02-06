namespace NoFuture.Rand.Law.US.Contracts.Breach
{
    /// <inheritdoc />
    public abstract class StandardsBase<T> : DilemmaBase<T> where T : IObjectiveLegalConcept
    {
        protected StandardsBase(IContract<T> contract) : base(contract)
        {
        }


        protected internal abstract bool StandardsTest(IObjectiveLegalConcept a, IObjectiveLegalConcept b);

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
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
