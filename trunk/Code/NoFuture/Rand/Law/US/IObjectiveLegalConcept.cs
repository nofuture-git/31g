using System.Collections.Generic;

namespace NoFuture.Rand.Law.US
{
    public interface IObjectiveLegalConcept
    {
        /// <summary>
        /// Subjective, inner thoughts, feelings, etc. 
        /// Objective, outward actions, works, deeds, etc.
        /// </summary>
        /// <param name="promisor"></param>
        /// <param name="promisee"></param>
        /// <returns></returns>
        bool IsValid(ILegalPerson promisor, ILegalPerson promisee);

        void AddAuditEntry(string msg);

        void AddAuditEntryRange(IEnumerable<string> msgs);

        IEnumerable<string> GetAuditEntries();
    }
}
