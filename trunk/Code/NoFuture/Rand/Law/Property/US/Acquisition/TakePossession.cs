using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US.Acquisition
{
    /// <summary>
    /// the moment in time when possession is achieved ... includes acts and thoughts of the would be possessor
    /// </summary>
    [Aka("acquisition by first possession")]
    public class TakePossession : PropertyBase
    {
        public TakePossession(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public IAct ClaimantAction { get; set; }
        public IIntent ClaimantIntent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (SubjectProperty == null)
            {
                AddReasonEntry($"{nameof(SubjectProperty)} is unassigned");
                return false;
            }

            if (PropertyOwnerIsInPossession(persons))
                return true;
            if (PropertyOwnerIsSubjectPerson(persons))
                return true;

            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            if (ClaimantAction == null && ClaimantIntent == null)
            {
                AddReasonEntry($"{title} {subj.Name}, both {nameof(ClaimantAction)} " +
                               $"and {nameof(ClaimantIntent)} are unassigned.");
                return false;
            }

            if (ClaimantAction != null && !ClaimantAction.IsValid(persons))
            {
                AddReasonEntryRange(ClaimantAction.GetReasonEntries());
                return false;
            }

            if (ClaimantIntent != null && !ClaimantIntent.IsValid(persons))
            {
                AddReasonEntryRange(ClaimantIntent.GetReasonEntries());
                return false;
            }

            SubjectProperty.EntitledTo = subj;
            SubjectProperty.InPossessionOf = subj;

            return true;
        }
    }
}
