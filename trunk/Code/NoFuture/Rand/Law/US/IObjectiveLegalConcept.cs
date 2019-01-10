using System.Collections.Generic;

namespace NoFuture.Rand.Law.US
{
    public interface IObjectiveLegalConcept
    {
        /// <summary>
        /// Subjective, inner thoughts, feelings, etc. 
        /// Objective, outward actions, works, deeds, etc.
        /// </summary>
        bool IsValid(ILegalPerson offeror, ILegalPerson offeree);

        void AddReasonEntry(string msg);

        void AddReasonEntryRange(IEnumerable<string> msgs);

        IEnumerable<string> GetReasonEntries();

        bool IsEnforceableInCourt { get; }
    }
}
