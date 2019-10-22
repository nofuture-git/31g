using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Terms
{
    /// <summary>
    /// The legal written document which transfers title from person(s) to person(s)
    /// </summary>
    public class DeedTerm : Term<ILegalProperty>, ILegalConcept
    {
        private readonly Rationale _rationale = new Rationale();

        public DeedTerm(string name, ILegalProperty reference) : base(name, reference)
        {
        }

        public DeedTerm(string name, ILegalProperty reference, params ITermCategory[] categories) : base(name, reference, categories)
        {
        }

        public virtual Predicate<ILegalPerson> IsSigned { get; set; } = lp => false;
        public virtual Predicate<ILegalPerson> IsWritten { get; set; } = lp => false;

        public bool IsConveyanceComplete { get; set; } = false;

        public virtual bool IsValid(params ILegalPerson[] persons)
        {

            var offeror = this.Offeror(persons);
            var offeree = this.Offeree(persons);

            if (offeror == null || offeree == null)
                return false;

            var entiltedTo = IsConveyanceComplete ? offeree as ILegalPerson : offeror;

            if (!this.PropertyOwnerIsInPossession(RefersTo, entiltedTo))
                return false;

            if (!this.PropertyOwnerIsSubjectPerson(RefersTo, entiltedTo))
                return false;

            var predicateTuple = new[]
            {
                Tuple.Create(IsSigned, nameof(IsSigned)),
                Tuple.Create(IsWritten, nameof(IsWritten)),
            };
            var parties = new ILegalPerson[] {offeror, offeree};

            foreach (var lp in parties)
            {
                foreach (var predicate in predicateTuple)
                {
                    if (!ValidatePredicate(lp, predicate.Item1, predicate.Item2))
                        return false;
                }
            }

            return true;
        }

        protected internal bool ValidatePredicate(ILegalPerson lp, Predicate<ILegalPerson> p, string nameofP)
        {
            if (lp == null || p == null)
                return false;

            if (!p(lp))
            {
                var title = lp.GetLegalPersonTypeName();
                AddReasonEntry($"{title} {lp.Name} {nameofP} is false");
                return false;
            }

            return true;
        }

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
    }
}
