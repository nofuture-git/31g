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

        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
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

            if (!GetAgreedTerms(offeror, offeree).Any())
                return false;

            return true;
        }

        /// <summary>
        /// Gets the subset of terms that have both the same name and meaning 
        /// </summary>
        /// <param name="offeror"></param>
        /// <param name="offeree"></param>
        /// <returns></returns>
        public virtual ISet<Term<object>> GetAgreedTerms(ILegalPerson offeror, ILegalPerson offeree)
        {
            //the shared terms between the two
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
            var agreedTerms = Term<object>.GetAgreedTerms(sorTerms, seeTerms);
            if (!agreedTerms.Any())
            {
                AddReasonEntry("there are no terms, with the same name," +
                               $" shared between {offeror.Name} and {offeree.Name}");
            }

            return agreedTerms;
        }

        /// <summary>
        /// Gets the symetric difference of the terms between offeror and offeree
        /// </summary>
        /// <param name="offeror"></param>
        /// <param name="offeree"></param>
        /// <returns></returns>
        public virtual ISet<Term<object>> GetAdditionalTerms(ILegalPerson offeror, ILegalPerson offeree)
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
            var additionalTerms = Term<object>.GetAdditionalTerms(sorTerms, seeTerms);
            if (!additionalTerms.Any())
            {
                AddReasonEntry("there are no additional terms between " +
                               $" {offeror.Name} and {offeree.Name}");
            }
            return additionalTerms;
        }

        /// <summary>
        /// Gets the subset of terms which have the same name.
        /// </summary>
        /// <param name="offeror"></param>
        /// <param name="offeree"></param>
        /// <returns></returns>
        public virtual ISet<Term<object>> GetInNameAgreedTerms(ILegalPerson offeror, ILegalPerson offeree)
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

            var agreedTerms = Term<object>.GetInNameAgreedTerms(sorTerms, seeTerms);
            if (!agreedTerms.Any())
            {
                AddReasonEntry($"there are no terms shared between {offeror.Name} and {offeree.Name}");
                return new HashSet<Term<object>>();
            }
            return agreedTerms;
        }
    }
}