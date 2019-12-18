using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Acquisition.Found
{
    /// <summary>
    /// When possession is voluntarily forsaken by the owner
    /// </summary>
    public class AbandonedProperty : PropertyBase
    {
        public AbandonedProperty(Func<IEnumerable<ILegalPerson>, ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        [EtymologyNote("Latin", "'re' + 'linquere'", "back + to leave")]
        public virtual IAct Relinquishment { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subj.GetLegalPersonTypeName();

            if (base.PropertyOwnerIsInPossession(persons))
                return false;

            if (Relinquishment == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(Relinquishment)} is unassigned");
                return false;
            }

            if (!Relinquishment.IsValid(persons))
            {
                AddReasonEntryRange(Relinquishment.GetReasonEntries());
                return false;
            }

            SubjectProperty = new ResDerelictae(SubjectProperty) {IsEntitledTo = lp => false, IsInPossessionOf = lp => false};

            return true;
        }
    }
}
