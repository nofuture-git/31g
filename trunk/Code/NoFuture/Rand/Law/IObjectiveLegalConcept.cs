
namespace NoFuture.Rand.Law
{
    public interface IObjectiveLegalConcept :  IReasonable
    {
        /// <summary>
        /// Subjective, inner thoughts, feelings, etc. 
        /// Objective, outward actions, works, deeds, etc.
        /// </summary>
        bool IsValid(ILegalPerson offeror, ILegalPerson offeree);

        bool IsEnforceableInCourt { get; }
    }
}
