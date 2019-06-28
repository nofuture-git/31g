using System;
using System.Linq;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Property.US
{
    public abstract class PropertyConsent : UnoHomine
    {
        protected PropertyConsent(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        protected PropertyConsent() : this(ExtensionMethods.Defendant) { }

        public virtual IConsent Consent { get; set; }

        public virtual ILegalProperty SubjectProperty { get; set; }

        /// <summary>
        /// The expected result of <see cref="Consent"/> - default
        /// is false (i.e. the thief did not have consent to take such-and-such).
        /// </summary>
        protected virtual bool ConsentExpectedAs { get; set; } = false;

        /// <summary>
        /// Tests that <see cref="Consent"/> was not given by <see cref="SubjectProperty"/> owner
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        protected virtual bool WithoutConsent(ILegalPerson[] persons)
        {
            //is all the dependencies present
            if (SubjectProperty?.EntitledTo == null || Consent == null
                                                    || persons == null
                                                    || !persons.Any())
                return true;

            //if all the people are licensed or invited then consent is implied by the use of type (label)
            if (persons.Where(v => !v.IsSamePerson(SubjectProperty.EntitledTo)).All(p => p is ILicensee))
            {
                AddReasonEntry($"all non-owner persons implement {nameof(ILicensee)} interface type");
                return false;
            }

            //did the caller pass in any IVictim types
            var victims = persons.Victims().ToList();
            if (!victims.Any())
            {
                return true;
            }

            //is any of our victims also the owner of the property
            var ownerVictims = victims.Where(v => v.IsSamePerson(SubjectProperty.EntitledTo)).ToList();
            if (!ownerVictims.Any())
            {
                AddReasonEntry($"of {nameof(IVictim)}s named " +
                               $"{string.Join(",", victims.Select(v => v.Name))}, " +
                               $"none are {SubjectProperty.EntitledTo.Name}");
                return true;
            }

            foreach (var ownerVictim in ownerVictims)
            {
                var validConsent = Consent.IsValid(ownerVictim);
                AddReasonEntryRange(Consent.GetReasonEntries());
                //did the owner victim in fact give consent 
                if (validConsent != ConsentExpectedAs)
                {
                    AddReasonEntry($"owner-victim {ownerVictim.Name}, {nameof(Consent)} {nameof(IsValid)} " +
                                   $"is {validConsent}, it was expected to be {ConsentExpectedAs} " +
                                   $"for property {SubjectProperty}");
                    return false;
                }
            }

            return true;
        }

        protected virtual bool PropertyOwnerIsSubjectPerson(ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();
            if (SubjectProperty == null)
            {
                AddReasonEntry($"{title}, {subj.Name}, {nameof(SubjectProperty)} is unassigned");
                return false;
            }

            if (SubjectProperty.EntitledTo == null)
            {
                AddReasonEntry($"{title}, {subj.Name}, {nameof(SubjectProperty)} named " +
                               $"{SubjectProperty.Name}, {nameof(SubjectProperty.EntitledTo)} is unassigned");
                return false;
            }

            var isOwner = subj.IsSamePerson(SubjectProperty.EntitledTo);
            var isIsNot = isOwner ? " is owner " : " is not owner ";
            AddReasonEntry(
                $"{title}, {subj.Name}, {isIsNot} " +
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
                AddReasonEntry($"{title}, {subj.Name}, {nameof(SubjectProperty)} is unassigned");
                return false;
            }

            if (SubjectProperty.InPossessionOf == null)
            {
                AddReasonEntry($"{title}, {subj.Name}, {nameof(SubjectProperty)} named " +
                               $"{SubjectProperty.Name}, {nameof(SubjectProperty.InPossessionOf)} is unassigned");
                return false;
            }

            var hasPossession = subj.IsSamePerson(SubjectProperty.InPossessionOf);
            var isIsNot = hasPossession ? " is in possession " : " is not in possession ";
            AddReasonEntry(
                $"{title}, {subj.Name}, {isIsNot} " +
                $"of {SubjectProperty.GetType().Name} " +
                $"named '{SubjectProperty.Name}'");

            return hasPossession;
        }
    }
}
