using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law
{
    /// <inheritdoc cref="ILegalPerson"/>
    public class LegalPerson : VocaBase, ILegalPerson
    {
        private readonly List<string> _reasons = new List<string>();

        public LegalPerson()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public LegalPerson(string name) : base(name)
        {
            if(string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

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

        public static IEnumerable<string> GetNames(params ILegalPerson[] persons)
        {
            var names = new List<string>();
            if (persons == null)
                return names;

            foreach (var person in persons)
            {
                var name = person?.Name;
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                if (names.Any(n => string.Equals(n, name)))
                    continue;

                names.Add(name);
            }

            return names;
        }
    }
}
