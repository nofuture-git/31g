
namespace NoFuture.Rand.Law
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
    /// <![CDATA[
    /// objective test: the object itself viewed from the prespective of a reasonable person
    /// subjective test: the object viewed from the perspective of the subject
    /// ]]>
    /// </summary>
    public interface ILegalConcept :  IRationale
    {
        /// <summary>
        /// <![CDATA[valid: sufficiently supported by facts or authority (from Latin 'valere' "be strong") ]]>
        /// </summary>
        bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null);

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
