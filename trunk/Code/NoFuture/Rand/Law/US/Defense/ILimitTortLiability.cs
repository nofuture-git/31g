using System;

namespace NoFuture.Rand.Law.US.Defense
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
    public interface ILimitTortLiability : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[
        /// an exculpatory agreement may affect the public interest 
        /// adversely even if some of the Tunkl factors are not satisfied
        /// ]]>
        /// </summary>
        int NumberNeededToTestTrue { get; set; }

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [1] 
        /// The agreement concerns a business of a type generally thought suitable for 
        /// public regulation.
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsSuitableForPublicRegulation { get; set; }

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
        Predicate<ILegalPerson> IsImportantPublicService { get; set; }

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [3] 
        /// The party holds himself out as willing to perform this service for any 
        /// member of the public who seeks it, or at least for any member coming 
        /// within certain established standards.
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsOfferToAnyMemberOfPublic { get; set; }

        /// <summary>
        /// <![CDATA[
        /// Tunkl factors [4] 
        /// As a result of the essential nature of the service, in the economic setting 
        /// of the transaction, the party invoking exculpation possesses a decisive 
        /// advantage of bar gaining strength against any member of the public who 
        /// seeks his services.
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsAdvantageOverMemberOfPublic { get; set; }

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
        Predicate<ILegalPerson> IsStandardizedAdhesion { get; set; }

        /// <summary>
        /// shifts the tort liability from tortfeasor to injured person
        /// <![CDATA[
        /// Tunkl factors [6] 
        /// Finally, as a result of the transaction, the person or property of the 
        /// purchaser is placed under the control of the seller, subject to the risk 
        /// of carelessness by the seller or his agents.
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsSubjectToSellerCarelessness { get; set; }
    }
}