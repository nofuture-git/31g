using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law
{
    /// <inheritdoc />
    public abstract class LegalConcept : ILegalConcept
    {
        private readonly List<string> _reasons = new List<string>();

        public abstract bool IsValid(params ILegalPerson[] persons);

        public virtual bool IsEnforceableInCourt => true;

        protected virtual void ClearReasons()
        {
            _reasons.Clear();
        }

        public virtual IEnumerable<string> GetReasonEntries()
        {
            return _reasons.ToArray();
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

        public virtual bool EquivalentTo(object obj)
        {
            return Equals(obj);
        }
    }
}
