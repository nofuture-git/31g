using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Contract.US
{
    public class MutualAssent : LegalConcept, IAssentTerms, IAssent
    {
        public override bool IsEnforceableInCourt => true;

        /// <summary>
        /// Is invoked twice, once for offeror and again for offeree.
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

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);
            if (offeree == null || offeror == null)
                return false;

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
        public virtual ISet<Term<object>> GetAgreedTerms(ILegalPerson offeror, ILegalPerson offeree)
        {
            return ExtensionMethods.GetAgreedTerms(this, offeror, offeree);
        }

        /// <summary>
        /// Gets the symmetric difference of the terms between offeror and offeree
        /// </summary>
        /// <param name="offeror"></param>
        /// <param name="offeree"></param>
        /// <returns></returns>
        public virtual ISet<Term<object>> GetAdditionalTerms(ILegalPerson offeror, ILegalPerson offeree)
        {
            return ExtensionMethods.GetAdditionalTerms(this, offeror, offeree);
        }

        /// <summary>
        /// Gets the subset of terms which have the same name.
        /// </summary>
        /// <param name="offeror"></param>
        /// <param name="offeree"></param>
        /// <returns></returns>
        public virtual ISet<Term<object>> GetInNameAgreedTerms(ILegalPerson offeror, ILegalPerson offeree)
        {
            return ExtensionMethods.GetInNameAgreedTerms(this, offeror, offeree);
        }
    }
}