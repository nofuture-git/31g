using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Criminal.US.Elements.AgainstProperty;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Defense
{
    /// <summary>
    /// a shopkeeper is allowed to detain a suspected shoplifter on store
    /// property for a reasonable period of time, so long as the shopkeeper
    /// has cause to believe that the person detained in fact committed, or
    /// attempted to commit, theft 
    /// </summary>
    [Aka("claims of right", "recapture privilege")]
    public class ShopkeeperPrivilege : AgainstPropertyBase
    {
        private readonly Rationale _rationale = new Rationale();

        public ShopkeeperPrivilege() : this (ExtensionMethods.Tortfeasor) {  }

        public ShopkeeperPrivilege(Func<ILegalPerson[], ILegalPerson> getSubjectPerson): base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsReasonableCause { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsReasonableManner { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsReasonableTime { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var person = GetSubjectPerson(persons);
            if (person == null)
                return false;
            var personType = person.GetLegalPersonTypeName();

            var rslt = WithoutConsent(persons);

            if (!IsReasonableCause(person))
            {
                AddReasonEntry($"{personType} {person.Name}, {nameof(IsReasonableCause)} is false");
                rslt = false;
            }

            if (!IsReasonableManner(person))
            {
                AddReasonEntry($"{personType} {person.Name}, {nameof(IsReasonableManner)} is false");
                rslt = false; ;
            }

            if (!IsReasonableTime(person))
            {
                AddReasonEntry($"{personType} {person.Name}, {nameof(IsReasonableTime)} is false");
                rslt = false; ;
            }

            return rslt;
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

        public override string ToString()
        {
            return _rationale.ToString();
        }

        #endregion
    }
}
