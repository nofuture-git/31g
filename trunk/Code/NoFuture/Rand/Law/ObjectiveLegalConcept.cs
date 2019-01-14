using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US
{
    public abstract class ObjectiveLegalConcept : IObjectiveLegalConcept, IReasonable
    {
        private readonly List<string> _reasons = new List<string>();

        public abstract bool IsValid(ILegalPerson offeror, ILegalPerson offeree);

        public abstract bool IsEnforceableInCourt { get; }

        public virtual IEnumerable<string> GetReasonEntries()
        {
            return _reasons;
        }

        public virtual void AddReasonEntry(string msg)
        {
            if(!string.IsNullOrWhiteSpace(msg))
                _reasons.Add(msg);
        }

        public virtual void AddReasonEntryRange(IEnumerable<string> msgs)
        {
            if (msgs == null)
                return;
            foreach(var msg in msgs)
                AddReasonEntry(msg);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _reasons);
        }
    }
}
