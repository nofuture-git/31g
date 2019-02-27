using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Contract.US
{
    public interface IAssent : ILegalConcept
    {
        /// <summary>
        /// Assent: to agree with proposition(s) often with enthusiasm
        /// Consent: to grant permission for some action often with reluctance
        /// </summary>
        /// <remarks>
        /// src [LUCY v. ZEHMER Supreme Court of Virginia 196 Va. 493; 84 S.E.2d 516 (1954)]
        /// <![CDATA[
        /// If his words and acts, judged by a reasonable standard, manifest an intention 
        /// to agree, it is immaterial what may be the real but unexpressed state of his mind.
        /// ]]>
        /// </remarks>
        Predicate<ILegalPerson> IsApprovalExpressed { get; set; }

        Func<ILegalPerson, ISet<Term<object>>> TermsOfAgreement { get; set; }

        ISet<Term<object>> GetAgreedTerms(ILegalPerson offeror, ILegalPerson offeree);

        ISet<Term<object>> GetAdditionalTerms(ILegalPerson offeror, ILegalPerson offeree);

        ISet<Term<object>> GetInNameAgreedTerms(ILegalPerson offeror, ILegalPerson offeree);
    }
}
