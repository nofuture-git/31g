﻿using System;
using System.Collections.Generic;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Found
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

            if (!WithoutConsent(persons))
                return false;

            if (base.PropertyOwnerIsInPossession(persons))
                return false;
            if (OwnersAction == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(OwnersAction)} is unassigned");
                return false;
            }

            if (OwnersAction.IsVoluntary(subj))
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

            return true;
        }
    }
}
