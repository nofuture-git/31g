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

        void AddAuditEntry(string msg);

        void AddAuditEntryRange(IEnumerable<string> msgs);

        IEnumerable<string> GetAuditEntries();

        bool IsEnforceableInCourt { get; }
    }
}
