using System;
using System.Linq;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Contracts
{
    public class MutualAssent : IObjectiveLegalConcept
    {
        private readonly List<string> _audit = new List<string>();
        public IList<string> Audit => _audit;

        /// <summary>
        /// Is invoked twice, once for promisor and again for promisee.
        /// The resulting pair of terms must equal each other for a contract to exist.
        /// </summary>
        /// <remarks>
        /// src [OSWALD v. ALLEN 417 F.2d 43 (2d Cir. 1969)]
        /// &quot;
        /// when any of the terms used to express an agreement is ambivalent, and 
        /// the parties understand it in different ways, there cannot be a 
        /// contract unless one of them should have been aware of the other's 
        /// understanding.
        /// &quot;
        /// </remarks>
        public Func<ILegalPerson, ISet<Term<object>>> TermsOfAgreement { get; set; }

        /// <summary>
        /// A predicate when given either formative party of the contract
        /// will return some outward expression of approval.
        /// </summary>
        public Predicate<ILegalPerson> IsApprovalExpressed { get; set; }

        public bool IsValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            if (promisor == null)
            {
                _audit.Add($"{nameof(promisor)} is null");
                return false;
            }

            if (promisee == null)
            {
                _audit.Add($"{nameof(promisee)} is null");
                return false;
            }

            if (IsApprovalExpressed == null)
            {
                _audit.Add($"{nameof(IsApprovalExpressed)} is null");
                return false;
            }

            if (TermsOfAgreement == null)
            {
                _audit.Add($"{nameof(TermsOfAgreement)} is null");
                return false;
            }

            if (!IsTermsOfAgreementValid(promisor, promisee))
                return false;

            if (!IsApprovalExpressed(promisor))
            {
                _audit.Add($"{promisor.Name} did not outwardly express approval");
                return false;
            }

            if (!IsApprovalExpressed(promisee))
            {
                _audit.Add($"{promisee.Name} did not outwardly express approval");
                return false;
            }

            return true;
        }

        protected internal bool IsTermsOfAgreementValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            var sorTerms = TermsOfAgreement?.Invoke(promisor);
            if (sorTerms == null || !sorTerms.Any())
            {
                _audit.Add($"{promisor.Name} has no terms");
                return false;
            }

            var seeTerms = TermsOfAgreement(promisee);
            if (seeTerms == null || !seeTerms.Any())
            {
                _audit.Add($"{promisee.Name} has no terms");
                return false;
            }

            //the shared terms between the two
            var agreedTerms = sorTerms.Where(oo => seeTerms.Any(ee => ee.Equals(oo))).Select(v => v.Name);
            if (!agreedTerms.Any())
            {
                _audit.Add($"there are no terms shared between {promisor.Name} and {promisee.Name}");
                return false;
            }
            foreach (var term in agreedTerms)
            {
                var promisorIdeaOfTerm = sorTerms.First(v => v.Name == term);
                var promiseeIdeaOfTerm = seeTerms.First(v => v.Name == term);

                if (!promiseeIdeaOfTerm?.EqualRefersTo(promisorIdeaOfTerm) ?? false)
                {
                    _audit.Add($"the term '{term}' does not have the same meaning between " +
                              $"{promisor.Name} and {promisee.Name}");
                    return false;
                }
            }

            return true;
        }
    }
}