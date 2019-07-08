using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Acquisition.Found
{
    /// <summary>
    /// pass out of possession involuntarily through neglect, carelessness or
    /// inadvertence [...] and does not, at any time thereafter, know where to find it
    /// </summary>
    public class LostProperty : AbandonedProperty
    {
        public LostProperty(Func<IEnumerable<ILegalPerson>, ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public Predicate<ILegalPerson> IsPropertyLocationKnown { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            if (base.PropertyOwnerIsInPossession(persons))
                return false;
            if (Relinquishment == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(Relinquishment)} is unassigned");
                return false;
            }

            if (Relinquishment.IsVoluntary(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(LostProperty)} " +
                               "requires the owner to have acted involuntarily");
                return false;
            }

            if (IsPropertyLocationKnown(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsPropertyLocationKnown)} is true");
                return false;
            }

            SubjectProperty = new ResDerelictae(SubjectProperty) { EntitledTo = null, InPossessionOf = null };

            return true;
        }
    }
}
