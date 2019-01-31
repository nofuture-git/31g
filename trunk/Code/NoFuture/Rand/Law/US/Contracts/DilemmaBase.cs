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
        protected DilemmaBase(IContract<T> contract)
        {
            Contract = contract;
        }

        protected internal ISet<Term<object>> AgreedTerms { get; set; }

        protected internal ISet<Term<object>> OfferorTerms { get; set; }

        protected internal ISet<Term<object>> OffereeTerms { get; set; }

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
