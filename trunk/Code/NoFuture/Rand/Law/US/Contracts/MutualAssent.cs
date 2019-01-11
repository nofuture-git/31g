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
        public virtual Func<ILegalPerson, ISet<Term<object>>> TermsOfAgreement { get; set; }

        /// <inheritdoc />
        public virtual Predicate<ILegalPerson> IsApprovalExpressed { get; set; }

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

            if (TermsOfAgreement == null)
            {
                AddReasonEntry("there is no terms defined on which to assent");
                return false;
            }

            if (!IsTermsEqualInNameAndFact(offeror, offeree))
                return false;

            return true;
        }

        /// <summary>
        /// Determines if there is a semantic gap in common names meaning two different things
        /// </summary>
        /// <param name="promisor"></param>
        /// <param name="offeree"></param>
        /// <returns></returns>
        protected internal virtual bool IsTermsEqualInNameAndFact(ILegalPerson promisor, ILegalPerson offeree)
        {
            //the shared terms between the two
            var agreedTerms = GetAgreedTerms(promisor,offeree).Select(v => v.Name);
            if (!agreedTerms.Any())
            {
                AddReasonEntry($"there are no terms shared between {promisor.Name} and {offeree.Name}");
                return false;
            }
            var sorTerms = TermsOfAgreement(promisor);
            var seeTerms = TermsOfAgreement(offeree);
            foreach (var term in agreedTerms)
            {
                var promisorIdeaOfTerm = sorTerms.First(v => v.Name == term);
                var promiseeIdeaOfTerm = seeTerms.First(v => v.Name == term);

                if (!promiseeIdeaOfTerm?.EqualRefersTo(promisorIdeaOfTerm) ?? false)
                {
                    AddReasonEntry($"the term '{term}' does not have the same meaning between " +
                              $"{promisor.Name} and {offeree.Name}");
                    return false;
                }
            }
            return true;
        }

        public virtual ISet<Term<object>> GetAgreedTerms(ILegalPerson offeror, ILegalPerson offeree)
        {
            var sorTerms = TermsOfAgreement?.Invoke(offeror);
            if (sorTerms == null || !sorTerms.Any())
            {
                AddReasonEntry($"{offeror.Name} has no terms");
                return new HashSet<Term<object>>();
            }

            var seeTerms = TermsOfAgreement(offeree);
            if (seeTerms == null || !seeTerms.Any())
            {
                AddReasonEntry($"{offeree.Name} has no terms");
                return new HashSet<Term<object>>();
            }

            //the shared terms between the two
            var agreedList = sorTerms.Where(oo => seeTerms.Any(ee => ee.Equals(oo))).ToList();
            if (!agreedList.Any())
            {
                AddReasonEntry($"there are no terms shared between {offeror.Name} and {offeree.Name}");
                return new HashSet<Term<object>>();
            }
            var agreedTerms = new HashSet<Term<object>>();
            foreach (var t in agreedList)
            {
                agreedTerms.Add(t);
            }

            return agreedTerms;
        }

        internal ISet<Term<object>> GetDistinctTerms(ILegalPerson ofThisPerson, ILegalPerson compared2ThisPerson)
        {
            var agreedTerms = GetAgreedTerms(ofThisPerson, compared2ThisPerson);
            if (!agreedTerms.Any())
                return new HashSet<Term<object>>();

            var fromTheseTerms = TermsOfAgreement(ofThisPerson);
            fromTheseTerms.ExceptWith(agreedTerms);
            return fromTheseTerms;
        }
    }
}