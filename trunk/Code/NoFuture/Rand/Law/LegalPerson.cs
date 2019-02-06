using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;

namespace NoFuture.Rand.Law
{
    /// <inheritdoc cref="ILegalPerson"/>
    public class LegalPerson : VocaBase, ILegalPerson, IRationale
    {
        private readonly List<string> _reasons = new List<string>();

        public LegalPerson() { }

        public LegalPerson(string name) : base(name) { }

        public LegalPerson(string name, string groupName) : base(name, groupName) { }

        public virtual IEnumerable<string> GetReasonEntries()
        {
            return _reasons;
        }

        public virtual void AddReasonEntry(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
                _reasons.Add(msg);
        }

        public virtual void AddReasonEntryRange(IEnumerable<string> msgs)
        {
            if (msgs == null)
                return;
            foreach (var msg in msgs)
                AddReasonEntry(msg);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _reasons);
        }
    }
}
