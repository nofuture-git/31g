using System;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToAssent
{
    /// <summary>
    /// <![CDATA[an assertion that is not in accordance with the facts]]>
    /// </summary>
    /// <remarks>
    /// <![CDATA[ src: ALABI v. DHL AIRWAYS, INC. Superior Court of Delaware, New Castle 583 A.2d 1358 (Del. Super. 1990)]]>
    /// </remarks>
    public class Misrepresentation<T> : DefenseBase<T> where T : IObjectiveLegalConcept
    {
        public Misrepresentation(IContract<T> contract) : base(contract) { }

        /// <summary>
        /// <![CDATA[
        /// (1) A misrepresentation is fraudulent if the maker intends his assertion to 
        /// induce a party to manifest his assent and the maker (a), (b) or (c)
        /// 
        /// (2) A misrepresentation is material if it would be likely to induce a 
        /// reasonable person to manifest his assent, or if the maker knows that it 
        /// would be likely to induce the recipient to do so
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsAssertionToInduceAssent { get; set; } = llp => false;

        /// <summary>
        /// <![CDATA[(a) knows or believes that the assertion is not in accord with the facts]]>
        /// </summary>
        public Predicate<ILegalPerson> IsNotAssertionInAccord2Facts { get; set; } = llp => false;

        /// <summary>
        /// <![CDATA[(b) does not have the confidence that he states or implies in the truth of the assertion]]>
        /// </summary>
        public Predicate<ILegalPerson> IsNotAssertionInConfidence2Truth { get; set; } = llp => false;

        /// <summary>
        /// <![CDATA[(c) knows that he does not have the basis that he states or implies for the assertion]]>
        /// </summary>
        public Predicate<ILegalPerson> IsNotAssertionInBasis2ImpliedStatement { get; set; } = llp => false;

        public virtual bool IsFraudulent(ILegalPerson lp)
        {
            if (!IsAssertionToInduceAssent(lp))
                return false;

            AddReasonEntry($"{lp?.Name} intends his/her assertion to " +
                           "induce a party to manifest assent");
            if (IsNotAssertionInAccord2Facts(lp))
            {
                AddReasonEntry($"and, {lp?.Name} knows or believes that the " +
                               "assertion is not in accord with the facts");
                return true;
            }
            if (IsNotAssertionInConfidence2Truth(lp))
            {
                AddReasonEntry($"and, {lp?.Name} does not have the confidence " +
                               "that he/she states or implies in the truth of the assertion");
                return true;
            }
            if (IsNotAssertionInBasis2ImpliedStatement(lp))
            {
                AddReasonEntry($"and, {lp?.Name} knows that he/she does not have " +
                               "the basis that he/she states or implies for the assertion");
                return true;
            }

            return false;
        }

        public virtual bool IsMaterial(ILegalPerson lp)
        {
            return IsAssertionToInduceAssent(lp);
        }

        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            return IsFraudulent(offeror) || IsFraudulent(offeree) || IsMaterial(offeror) || IsMaterial(offeree);
        }

    }
}
