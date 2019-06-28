using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Found
{
    /// <summary>
    /// When possession is voluntarily forsaken by the owner
    /// </summary>
    public class AbandonedProperty : PropertyConsent
    {
        public AbandonedProperty(Func<IEnumerable<ILegalPerson>, ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public virtual IAct OwnersAction { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            if (!WithoutConsent(persons))
                return false;

            if (base.PropertyOwnerIsInPossession(persons))
                return false;

            if (OwnersAction == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(OwnersAction)} is unassigned");
                return false;
            }

            if (!OwnersAction.IsValid(persons))
            {
                AddReasonEntryRange(OwnersAction.GetReasonEntries());
                return false;
            }

            return true;
        }
    }
}
