using System;

namespace NoFuture.Rand.Law.US.Defense
{
    /// <summary>
    /// <![CDATA[
    /// intoxication which (a) is not self-induced ... is an affirmative defense if 
    /// by reason of such intoxication 211 Criminal Law the actor at the time of his 
    /// conduct lacks substantial capacity either to appreciate its criminality 
    /// [wrongfulness] or  to conform his conduct to the requirements of law 
    /// (Model Penal Code § 2.08 (4)).
    /// ]]>
    /// </summary>
    public interface IIntoxication : ILegalConcept
    {
        /// <summary>
        /// <![CDATA[
        /// Involuntary intoxication could affect the defendant's ability to form 
        /// criminal intent, thus negating specific intent
        /// ]]>
        /// </summary>
        Predicate<ILegalPerson> IsInvoluntary { get; set; }

        Predicate<ILegalPerson> IsIntoxicated { get; set; }
    }
}