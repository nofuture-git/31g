using System;
using System.Collections.Generic;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.Enums;

namespace NoFuture.Rand.Law
{
    public class LegalProperty : VocaBase, ILegalProperty
    {
        private decimal? _propertyValue;
        private Predicate<ILegalPerson> _isEntitledTo = lp => false;
        private Predicate<ILegalPerson> _isInPossessionOf = lp => false;

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

        public LegalProperty(ILegalProperty property)
        {
            if (property == null)
                return;

            CopyNamesFrom(property);
            _isEntitledTo = property.IsEntitledTo ?? (lp => false);
            _isInPossessionOf = property.IsInPossessionOf ?? (lp => false);
            _propertyValue = property.CurrentPropertyValue;
            foreach (var msg in property.GetReasonEntries())
            {
                _reasons.Add(msg);
            }
        }

        public virtual Predicate<ILegalPerson> IsEntitledTo
        {
            get => _isEntitledTo;
            set => _isEntitledTo = value;
        }

        public virtual Predicate<ILegalPerson> IsInPossessionOf
        {
            get => _isInPossessionOf;
            set => _isInPossessionOf = value;
        }

        public virtual decimal? CurrentPropertyValue => PropertyValue(DateTime.Now);

        public virtual Func<DateTime?, decimal?> PropertyValue { get; set; } = dt => 0m;

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
            var val = CurrentPropertyValue.GetValueOrDefault(0m);
            return Convert.ToInt32(Math.Round(val, 0));
        }
    }
}
