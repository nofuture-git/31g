using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.US
{
    /// <summary>
    /// Composition class of legal-concept and term-category
    /// </summary>
    public abstract class TermLegalConcept: TermCategory, ILegalConcept
    {
        public Func<ILegalPerson[], ILegalPerson> GetSubjectPerson { get; set; }

        protected TermLegalConcept(Func<ILegalPerson[], ILegalPerson> getSubjectPerson)
        {
            GetSubjectPerson = getSubjectPerson;
        }
        public abstract bool IsValid(params ILegalPerson[] persons);

        #region IRationale IS-A HAS-A

        private readonly IRationale _rationale = new Rationale();
        public IEnumerable<string> GetReasonEntries()
        {
            return _rationale.GetReasonEntries();
        }

        public void AddReasonEntry(string msg)
        {
            _rationale.AddReasonEntry(msg);
        }

        public void AddReasonEntryRange(IEnumerable<string> msgs)
        {
            _rationale.AddReasonEntryRange(msgs);
        }

        public void ClearReasons()
        {
            _rationale.ClearReasons();
        }

        public bool IsEnforceableInCourt { get; } = true;
        public bool EquivalentTo(object obj)
        {
            return Equals(obj);
        }

        #endregion
    }
}
