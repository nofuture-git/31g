using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US.Contracts.Defense.ToPublicPolicy
{
    /// <summary>
    /// contracts that limit tort liability
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// exculpatory provisions undermine the policy considerations governing our 
    /// tort system - with the public bearing the cost of the resulting injuries
    /// ]]>
    /// </remarks>
    public class ByLimitTortLiability<T> : DefenseBase<T> where T : ILegalConcept
    {
        public ByLimitTortLiability(IContract<T> contract) : base(contract)
        {
        }
        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (!base.IsValid(offeror, offeree))
                return false;

            var cnt = 0;
            foreach (var kvp in GetPredicate2ReasonString())
            {
                var p = kvp.Key;
                if (p(offeror) || p(offeree))
                {
                    AddReasonEntry(kvp.Value);
                    cnt += 1;
                }
            }

            return cnt >= NumberNeededToTestTrue;
        }

        /// <summary>
        /// <![CDATA[
        /// an exculpatory agreement may affect the public interest 
        /// adversely even if some of the Tunkl factors are not satisfied
        /// ]]>
        /// </summary>
        public int NumberNeededToTestTrue { get; set; } = 5;

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [1] 
        /// The agreement concerns a business of a type generally thought suitable for 
        /// public regulation.
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsSuitableForPublicRegulation { get; set; } = c => false;

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [2] 
        /// The party seeking exculpation is engaged in performing a service of great 
        /// importance to the public, which is often a matter of practical necessity 
        /// for some members of the public.
        /// ]]>
        /// </summary>
        /// <remarks>
        /// e.g. medical services, child care, banking, real estate services, etc.
        /// </remarks>
        public Predicate<ILegalPerson> IsImportantPublicService { get; set; } = c => false;

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [3] 
        /// The party holds himself out as willing to perform this service for any 
        /// member of the public who seeks it, or at least for any member coming 
        /// within certain established standards.
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsOfferToAnyMemberOfPublic { get; set; } = c => false;

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [4] 
        /// As a result of the essential nature of the service, in the economic setting 
        /// of the transaction, the party invoking exculpation possesses a decisive 
        /// advantage of bar gaining strength against any member of the public who 
        /// seeks his services.
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsAdvantageOverMemberOfPublic { get; set; } = c => false;

        /// <summary>
        /// A preprinted form with a bunch paragraphs in legal-speak that is presented as 
        /// take-it-or-leave-it.
        /// <![CDATA[
        /// Tunkl factors [5] 
        /// In exercising a superior bargaining power the party confronts the public 
        /// with a standardized adhesion contract of exculpation, and makes no 
        /// provision whereby a purchaser may pay additional reasonable fees and 
        /// obtain protection against negligence.
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsStandardizedAdhesion { get; set; } = c => false;

        /// <summary>
        /// shifts the tort liability from tortfeasor to injured person
        /// <![CDATA[
        /// Tunkl factors [6] 
        /// Finally, as a result of the transaction, the person or property of the 
        /// purchaser is placed under the control of the seller, subject to the risk 
        /// of carelessness by the seller or his agents.
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsSubjectToSellerCarelessness { get; set; } = c => false;

        protected internal virtual Dictionary<Predicate<ILegalPerson>, string> GetPredicate2ReasonString()
        {
            return new Dictionary<Predicate<ILegalPerson>, string>
            {
                {IsSuitableForPublicRegulation, "the agreement suitable for public regulation"},
                {IsImportantPublicService, "the service is of importance to the public"},
                {IsOfferToAnyMemberOfPublic, "the offer is open to any memeber of public"},
                {IsAdvantageOverMemberOfPublic, "there is a decisive advantage over any member of the public"},
                {IsStandardizedAdhesion, "the agreement involves a standardized exculpation form"},
                {IsSubjectToSellerCarelessness, "purchaser is subject to risk by seller carelessness"},
            };
        }
    }
}
