using System;
using System.Linq;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Contracts
{
    public class MutualAssent : ObjectiveLegalConcept, IAssent
    {
        public override bool IsEnforceableInCourt => true;

        /// <summary>
        /// Is invoked twice, once for promisor and again for promisee.
        /// The resulting pair of terms must equal each other in both 
        /// name and reference for a contract to exist.
        /// </summary>
        /// <remarks>
        /// src [OSWALD v. ALLEN United States Court of Appeals for the Second Circuit 417 F.2d 43 (2d Cir. 1969)]
        /// <![CDATA[
        /// when any of the terms used to express an agreement is ambivalent, and
        /// the parties understand it in different ways, there cannot be a 
        /// contract unless one of them should have been aware of the other's 
        /// understanding.
        /// ]]>
        /// </remarks>
        public Func<ILegalPerson, ISet<Term<object>>> TermsOfAgreement { get; set; }

        /// <inheritdoc />
        public Predicate<ILegalPerson> IsApprovalExpressed { get; set; }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (offeror == null)
            {
                AddReasonEntry($"{nameof(offeror)} is null");
                return false;
            }

            if (offeree == null)
            {
                AddReasonEntry($"{nameof(offeree)} is null");
                return false;
            }

            if (IsApprovalExpressed == null)
            {
                AddReasonEntry($"{nameof(IsApprovalExpressed)} is null");
                return false;
            }

            if (TermsOfAgreement == null)
            {
                AddReasonEntry($"{nameof(TermsOfAgreement)} is null");
                return false;
            }

            if (!IsTermsOfAgreementValid(offeror, offeree))
                return false;

            if (!IsApprovalExpressed(offeror))
            {
                AddReasonEntry($"{offeror.Name} did not outwardly express approval");
                return false;
            }

            if (!IsApprovalExpressed(offeree))
            {
                AddReasonEntry($"{offeree.Name} did not outwardly express approval");
                return false;
            }

            return true;
        }

        protected internal bool IsTermsOfAgreementValid(ILegalPerson promisor, ILegalPerson promisee)
        {
            var sorTerms = TermsOfAgreement?.Invoke(promisor);
            if (sorTerms == null || !sorTerms.Any())
            {
                AddReasonEntry($"{promisor.Name} has no terms");
                return false;
            }

            var seeTerms = TermsOfAgreement(promisee);
            if (seeTerms == null || !seeTerms.Any())
            {
                AddReasonEntry($"{promisee.Name} has no terms");
                return false;
            }

            //the shared terms between the two
            var agreedTerms = sorTerms.Where(oo => seeTerms.Any(ee => ee.Equals(oo))).Select(v => v.Name);
            if (!agreedTerms.Any())
            {
                AddReasonEntry($"there are no terms shared between {promisor.Name} and {promisee.Name}");
                return false;
            }
            foreach (var term in agreedTerms)
            {
                var promisorIdeaOfTerm = sorTerms.First(v => v.Name == term);
                var promiseeIdeaOfTerm = seeTerms.First(v => v.Name == term);

                if (!promiseeIdeaOfTerm?.EqualRefersTo(promisorIdeaOfTerm) ?? false)
                {
                    AddReasonEntry($"the term '{term}' does not have the same meaning between " +
                              $"{promisor.Name} and {promisee.Name}");
                    return false;
                }
            }

            return true;
        }
    }
}