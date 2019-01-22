
namespace NoFuture.Rand.Law
{
    /// <summary>
    /// Subjective, inner thoughts, feelings, etc. 
    /// Objective, outward actions, works, deeds, etc.
    /// </summary>
    public interface IObjectiveLegalConcept :  IReasonable
    {
        /// <summary>
        /// <![CDATA[valid: sufficiently supported by facts or authority (from Latin 'valere' "be strong") ]]>
        /// </summary>
        bool IsValid(ILegalPerson offeror, ILegalPerson offeree);

        bool IsEnforceableInCourt { get; }
    }
}
