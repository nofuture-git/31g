using System;

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
    public class ByLimitTortLiability<T> : DefenseBase<T>, IVoidable
    {
        public ByLimitTortLiability(IContract<T> contract) : base(contract)
        {
        }
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!base.IsValid(offeror, offeree))
                return false;

            throw new NotImplementedException();
        }

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [1] 
        /// The agreement concerns a business of a type generally thought suitable for 
        /// public regulation.
        /// ]]>
        /// </summary>
        public Predicate<IContract<T>> IsSuitableForPublicRegulation { get; set; } = c => false;

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
        public Predicate<IContract<T>> IsImportantPublicService { get; set; } = c => false;

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [3] 
        /// The party holds himself out as willing to perform this service for any 
        /// member of the public who seeks it, or at least for any member coming 
        /// within certain established standards.
        /// ]]>
        /// </summary>
        public Predicate<IContract<T>> IsOfferToAnyMemberOfPublic { get; set; } = c => false;

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [4] 
        /// As a result of the essential nature of the service, in the economic setting 
        /// of the transaction, the party invoking exculpation possesses a decisive 
        /// advantage of bar gaining strength against any member of the public who 
        /// seeks his services.
        /// ]]>
        /// </summary>
        public Predicate<IContract<T>> IsAdvantageOverMemberOfPublic { get; set; } = c => false;

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
        public Predicate<IContract<T>> IsStandardizedAdhesion { get; set; } = c => false;

        /// <summary>
        /// shifts the tort liability from tortfeasor to injured person
        /// <![CDATA[
        /// Tunkl factors [6] 
        /// Finally, as a result of the transaction, the person or property of the 
        /// purchaser is placed under the control of the seller, subject to the risk 
        /// of carelessness by the seller or his agents.
        /// ]]>
        /// </summary>
        public Predicate<IContract<T>> IsSubjectToSellerCarelessness { get; set; } = c => false;
    }
}
