using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements
{
    public abstract class AgitPropertyBase : LegalConcept
    {
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

            //did the caller pass in any IVictim types
            var victims = persons.Victims().ToList();
            if (!victims.Any())
                return true;

            //is any of our victims also the owner of the property
            var ownerVictims = victims.Where(v => VocaBase.Equals(v, SubjectProperty.EntitledTo)).ToList();
            if (!ownerVictims.Any())
                return true;

            foreach (var ownerVictim in ownerVictims)
            {
                var validConsent = Consent.IsValid(ownerVictim);
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

        protected virtual bool PropertyOwnerIsDefendant(ILegalPerson[] persons)
        {
            var defendant = persons.Defendant();
            if (defendant == null)
                return false;
            if (SubjectProperty?.EntitledTo == null)
                return false;

            var isOwner = VocaBase.Equals(defendant, SubjectProperty.EntitledTo) ||
                   ReferenceEquals(defendant, SubjectProperty.EntitledTo);
            if(isOwner)
                AddReasonEntry(
                    $"defendant, {defendant.Name}, is owner " +
                    $"of {SubjectProperty.GetType().Name} " +
                    $"named '{SubjectProperty.Name}'");

            return isOwner;
        }
    }
}
