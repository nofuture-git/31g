using System;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;
using NoFuture.Rand.Law.US.Property;

namespace NoFuture.Rand.Law.Tort.US.Elements.ReasonableCare
{
    /// <summary>
    /// The common law rules regarding if a duty of care is required of a land owner towards visitors
    /// </summary>
    public class OfLandOwner : PropertyConsent, IDuty
    {
        public OfLandOwner(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalProperty> IsAttractiveToChildren { get; set; } = p => false;

        /// <summary>
        /// Determines if all whose visitors among <see cref="persons"/> are owed a duty for standard of care 
        /// </summary>
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();
            if (!PropertyOwnerIsSubjectPerson(persons))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(PropertyOwnerIsSubjectPerson)} is false");
                return false;
            }

            var areGivenPermission = !WithoutConsent(persons);

            if (areGivenPermission)
                return true;

            var allChildren = persons.Where(v => !v.IsSamePerson(SubjectProperty.EntitledTo)).All(p => p is IChild);

            if (allChildren && IsAttractiveToChildren(SubjectProperty))
            {
                AddReasonEntry($"{title} {subj.Name} property '{SubjectProperty.Name}', " +
                               $"{nameof(IsAttractiveToChildren)} and all persons are " +
                               $"{nameof(IChild)} interface type");
                return true;
            }

            return false;
        }
    }
}
