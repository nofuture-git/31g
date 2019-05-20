using System;
using System.Collections.Generic;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;

namespace NoFuture.Rand.Law
{
    public class LegalProperty : VocaBase, ILegalProperty
    {
        private readonly List<string> _reasons = new List<string>();

        public LegalProperty()
        {
            base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public LegalProperty(string name) : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
                base.AddName(KindsOfNames.Legal, GetType().Name.ToUpper());
        }

        public LegalProperty(string name, string groupName) : base(name, groupName) { }

        public virtual ILegalPerson EntitledTo { get; set; }

        public virtual ILegalPerson InPossessionOf { get; set; }
        public virtual decimal? PropertyValue { get; set; }

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

        public virtual void ClearReasons()
        {
            _reasons.Clear();
        }

        public override string ToString()
        {
            return Name;
        }

        public int GetRank()
        {
            var val = PropertyValue.GetValueOrDefault(0m);
            return Convert.ToInt32(Math.Round(val, 0));
        }
    }
}
