using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Contracts
{
    public interface IAssent : IObjectiveLegalConcept
    {
        /// <summary>
        /// A predicate when given either formative party of the contract
        /// will return some outward expression of approval.
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
    }
}
