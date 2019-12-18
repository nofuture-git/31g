using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.Property.US.FormsOf;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Acquisition.Found
{
    /// <summary>
    /// is intentionally put into a certain place and later forgotten
    /// </summary>
    /// <remarks>
    /// a finder of <see cref="MislaidProperty"/> acquires no ownership rights
    /// in it, and, where such property is found upon
    /// another&apos;s premises [...] is required to turn it over 
    /// </remarks>
    public class MislaidProperty : LostProperty
    {
        public MislaidProperty(Func<IEnumerable<ILegalPerson>, ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

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
                AddReasonEntry($"{title} {subj.Name}, {nameof(MislaidProperty)} requires property was " +
                               "voluntarily placed somewhere but later forgotten");
                return false;
            }

            if (IsPropertyLocationKnown(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsPropertyLocationKnown)} is true");
                return false;
            }

            SubjectProperty = new ResDerelictae(SubjectProperty) { IsEntitledTo = lp => false, IsInPossessionOf = lp => false };

            return true;
        }
    }
}
