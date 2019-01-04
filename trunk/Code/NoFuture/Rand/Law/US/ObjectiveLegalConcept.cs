using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US
{
    public abstract class ObjectiveLegalConcept : IObjectiveLegalConcept
    {
        private readonly List<string> _audit = new List<string>();

        public abstract bool IsValid(ILegalPerson offeror, ILegalPerson offeree);

        public abstract bool IsEnforceableInCourt { get; }

        public virtual IEnumerable<string> GetAuditEntries()
        {
            return _audit;
        }

        public virtual void AddAuditEntry(string msg)
        {
            if(!string.IsNullOrWhiteSpace(msg))
                _audit.Add(msg);
        }

        public virtual void AddAuditEntryRange(IEnumerable<string> msgs)
        {
            if (msgs == null)
                return;
            foreach(var msg in msgs)
                AddAuditEntry(msg);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _audit);
        }
    }
}
