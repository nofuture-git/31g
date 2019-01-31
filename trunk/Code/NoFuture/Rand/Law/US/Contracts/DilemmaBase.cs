using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Law.US.Contracts
{
    /// <inheritdoc />
    /// <summary>
    /// A contract in dispute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DilemmaBase<T> : ObjectiveLegalConcept where T : IObjectiveLegalConcept
    {
        public virtual IContract<T> Contract { get; }

        public override bool IsEnforceableInCourt => true;

        /// <summary>
        /// Function to get the results of a given party
        /// </summary>
        public Func<ILegalPerson, T> ActualPerformance { get; set; } = lp => default(T);

        protected DilemmaBase(IContract<T> contract)
        {
            Contract = contract;
        }

        protected internal ISet<Term<object>> AgreedTerms { get; private set; }

        protected internal ISet<Term<object>> OfferorTerms { get; private set; }

        protected internal ISet<Term<object>> OffereeTerms { get; private set; }

        protected internal IObjectiveLegalConcept Offer { get; private set; }

        protected internal T Acceptance { get; private set; }

        protected internal IObjectiveLegalConcept OfferActual { get; private set; }

        protected internal T AcceptanceActual { get; private set; }

        protected internal bool TryGetActualOfferAcceptance(ILegalPerson offeror, ILegalPerson offeree)
        {

            OfferActual = ActualPerformance(offeror);
            if (OfferActual == null)
            {
                AddReasonEntry($"the offeror, {offeror.Name}, did not perform anything");
                return false;
            }

            AcceptanceActual = ActualPerformance(offeree);
            if (AcceptanceActual == null)
            {
                AddReasonEntry($"the offeree, {offeree.Name}, did not perform anything");
                return false;
            }

            return true;
        }

        protected internal bool TryGetOfferAcceptance(ILegalPerson offeror, ILegalPerson offeree)
        {
            Offer = Contract.Offer;
            Acceptance = Contract.Acceptance(Contract.Offer);
            if (Offer == null)
            {
                AddReasonEntry($"there is no offer from {offeror.Name}");
                return false;
            }

            if (Acceptance == null)
            {
                AddReasonEntry($"there is no return promise or performance given by {offeree.Name}");
                return false;
            }

            return true;
        }

        protected internal bool TryGetTerms(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (offeror == null || offeree == null)
            {
                AddReasonEntry("cannot resolve contract terms with both and offeror and offeree");
                return false;
            }

            if (Contract?.Assent == null)
            {
                AddReasonEntry("resolving ambiguous terms requires a contract with assent");
                return false;
            }

            AgreedTerms = Contract.Assent.GetInNameAgreedTerms(offeror, offeree);
            AddReasonEntryRange(Contract.Assent.GetReasonEntries());
            if (!AgreedTerms.Any())
            {
                return false;
            }
            OfferorTerms = Contract.Assent.TermsOfAgreement(offeror);
            OffereeTerms = Contract.Assent.TermsOfAgreement(offeree);

            return true;
        }
    }
}
