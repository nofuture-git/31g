using System;
using System.Linq;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Property.US
{
    public abstract class PropertyConsent : PropertyBase
    {
        protected PropertyConsent(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public virtual IConsent Consent { get; set; }

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
            if (SubjectProperty?.IsEntitledTo == null || Consent == null
                                                      || persons == null
                                                      || !persons.Any())
                return true;

            //if all the people are licensed or invited then consent is implied by the use of type (label)
            if (persons.Where(v => !SubjectProperty.IsEntitledTo(v)).All(p => p is ILicensee))
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
            var ownerVictims = victims.Where(v => SubjectProperty.IsEntitledTo(v)).ToList();
            if (!ownerVictims.Any())
            {
                AddReasonEntry($"of {nameof(IVictim)}s named " +
                               $"{string.Join(",", victims.Select(v => v.Name))}, " +
                               $"none are found {nameof(SubjectProperty.IsEntitledTo)} as true");
                return true;
            }

            foreach (var ownerVictim in ownerVictims)
            {
                var validConsent = Consent.IsValid(ownerVictim);
                AddReasonEntryRange(Consent.GetReasonEntries());
                //did the owner victim in fact give consent 
                if (validConsent != ConsentExpectedAs)
                {
                    AddReasonEntry($"owner-{ownerVictim.GetLegalPersonTypeName()} {ownerVictim.Name}, " +
                                   $"{nameof(Consent)} {nameof(IsValid)} " +
                                   $"is {validConsent}, it was expected to be {ConsentExpectedAs} " +
                                   $"for property {SubjectProperty.Name}");
                    return false;
                }
            }

            return true;
        }
    }
}
