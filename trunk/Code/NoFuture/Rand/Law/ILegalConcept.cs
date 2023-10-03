
using NoFuture.Law.Attributes;

namespace NoFuture.Law
{
    /// <summary>
    /// Subjective Test: the object viewed from the perspective of the subject
    /// </summary>
    public delegate bool SubjectivePredicate<in T>(T obj);

    /// <summary>
    /// Objective Test: the object viewed from the perspective of a reasonable person
    /// </summary>
    public delegate bool ObjectivePredicate<in T>(T obj);

    /// <summary>
    /// Main interface of various legal concepts 
    /// </summary>
    public interface ILegalConcept :  IRationale
    {
        /// <summary>
        /// <![CDATA[valid: sufficiently supported by facts or authority ]]>
        /// </summary>
        [EtymologyNote("latin", "valere", "be strong")]
        bool IsValid(params ILegalPerson[] persons);

        bool IsEnforceableInCourt { get; }

        /// <summary>
        /// <![CDATA[
        /// Those who think more of symmetry and logic in the development of legal rules than 
        /// of practical adaptation to the attainment of a just result will be troubled by a 
        /// classification where the lines of division are so wavering and blurred.
        /// ]]>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool EquivalentTo(object obj);
    }
}
