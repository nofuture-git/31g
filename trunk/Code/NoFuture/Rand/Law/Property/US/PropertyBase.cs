using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Property.US
{
    public abstract class PropertyBase : UnoHomine, ILegalConceptWithProperty
    {
        protected PropertyBase(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public virtual ILegalProperty SubjectProperty { get; set; }

        protected virtual bool PropertyOwnerIsSubjectPerson(ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();
            if (SubjectProperty == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(SubjectProperty)} is unassigned");
                return false;
            }

            var isOwner = SubjectProperty.EntitledTo != null && subj.IsSamePerson(SubjectProperty.EntitledTo);
            var isIsNot = isOwner ? "is owner" : "is not owner";
            AddReasonEntry(
                $"{title} {subj.Name}, {isIsNot} " +
                $"of {SubjectProperty.GetType().Name} " +
                $"named '{SubjectProperty.Name}'");

            return isOwner;
        }

        protected virtual bool PropertyOwnerIsInPossession(ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();
            if (SubjectProperty == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(SubjectProperty)} is unassigned");
                return false;
            }

            var hasPossession = SubjectProperty.InPossessionOf != null && subj.IsSamePerson(SubjectProperty.InPossessionOf);
            var isIsNot = hasPossession ? "is in possession" : "is not in possession";
            AddReasonEntry(
                $"{title} {subj.Name}, {isIsNot} " +
                $"of {SubjectProperty.GetType().Name} " +
                $"named '{SubjectProperty.Name}'");

            return hasPossession;
        }
    }
}
