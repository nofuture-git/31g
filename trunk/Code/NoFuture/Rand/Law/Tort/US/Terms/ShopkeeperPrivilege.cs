using System;
using System.Collections.Generic;

namespace NoFuture.Rand.Law.Tort.US.Terms
{
    /// <summary>
    /// a shopkeeper is allowed to detain a suspected shoplifter on store
    /// property for a reasonable period of time, so long as the shopkeeper
    /// has cause to believe that the person detained in fact committed, or
    /// attempted to commit, theft 
    /// </summary>
    public class ShopkeeperPrivilege : TermCategory, ILegalConcept
    {
        private readonly Rationale _rationale = new Rationale();

        protected override string CategoryName { get; } = "shopkeeper’s privilege";

        public bool IsValid(params ILegalPerson[] persons)
        {
            throw new NotImplementedException();
        }

        #region LegalConcept IS-A HAS-A

        public bool IsEnforceableInCourt { get; } = true;

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

        public bool EquivalentTo(object obj)
        {
            return _rationale.EquivalentTo(obj);
        }
        #endregion
    }
}
