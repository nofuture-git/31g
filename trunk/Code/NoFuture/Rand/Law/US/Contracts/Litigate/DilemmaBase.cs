using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Law.US.Contracts.Litigate
{
    /// <summary>
    /// base type for the various kinds of litigation dilemmas
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DilemmaBase<T> : LitigateBase<T>
    {
        protected DilemmaBase(IContract<T> contract) : base(contract)
        {
        }

        /// <summary>
        /// An option condition on any of the agreed terms which must be met.
        /// </summary>
        public Predicate<Term<object>> IsPrerequisite { get; set; } = t => true;

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

            var preque = IsPrerequisite ?? (t => true);
            if (!AgreedTerms.Any(agreedTerm => preque(agreedTerm)))
            {
                AddReasonEntry($"none of the terms between {offeror.Name} and {offeree.Name} " +
                               "satisfied the prerequisite condition.");
                return false;
            }
            OfferorTerms = Contract.Assent.TermsOfAgreement(offeror);
            OffereeTerms = Contract.Assent.TermsOfAgreement(offeree);

            return true;
        }
    }
}
