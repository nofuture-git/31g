using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Terms
{
    /// <summary>
    /// a shopkeeper is allowed to detain a suspected shoplifter on store
    /// property for a reasonable period of time, so long as the shopkeeper
    /// has cause to believe that the person detained in fact committed, or
    /// attempted to commit, theft 
    /// </summary>
    public class ShopkeeperPrivilege : TermCategory, ILegalConcept, IForce
    {
        private readonly Rationale _rationale = new Rationale();
        private Func<ILegalPerson[], ILegalPerson> _getSubjectPerson;

        public ShopkeeperPrivilege() : this (null) {  }

        public ShopkeeperPrivilege(Func<ILegalPerson[], ILegalPerson> getSubjectPerson)
        {
            _getSubjectPerson = getSubjectPerson ?? ExtensionMethods.Tortfeasor;
        }

        protected override string CategoryName { get; } = "shopkeeper’s privilege";

        public Predicate<ILegalPerson> IsReasonableCause { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsReasonableManner { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsReasonableTime { get; set; } = lp => false;

        public bool IsValid(params ILegalPerson[] persons)
        {
            var person = _getSubjectPerson(persons);
            if (person == null)
                return false;
            var personType = person.GetLegalPersonTypeName();
            if (!IsReasonableCause(person))
            {
                AddReasonEntry($"{personType} {person.Name}, {nameof(IsReasonableCause)} is false");
                return false;
            }

            if (!IsReasonableManner(person))
            {
                AddReasonEntry($"{personType} {person.Name}, {nameof(IsReasonableManner)} is false");
                return false;
            }

            if (!IsReasonableTime(person))
            {
                AddReasonEntry($"{personType} {person.Name}, {nameof(IsReasonableTime)} is false");
                return false;
            }

            return true;
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
