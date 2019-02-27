using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Criminal.US;
using NoFuture.Rand.Law.Criminal.US.Elements.Act;
using NoFuture.Rand.Law.Criminal.US.Elements.Intent;

namespace NoFuture.Rand.Law.Criminal.AgainstProperty.US.Elements.Theft
{
    /// <summary>
    /// Explanatory Note for Sections 223.1-223.9 of Model Penal Code
    /// </summary>
    public abstract class ConsolidatedTheft : CriminalBase, IActusReus
    {
        public virtual ILegalProperty SubjectOfTheft { get; set; }
        public virtual decimal? AmountOfTheft { get; set; }

        public virtual IConsent Consent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetDefendant(persons);
            if (defendant == null)
                return false;

            if (SubjectOfTheft == null)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(SubjectOfTheft)} is null");
                return false;
            }

            if (VocaBase.Equals(SubjectOfTheft.BelongsTo, defendant))
            {
                AddReasonEntry($"defendant, {defendant.Name}, is the owner of property {SubjectOfTheft}");
                return false;
            }

            if (!IsWithoutConsent(persons))
                return false;

            return true;
        }

        public virtual bool CompareTo(IMensRea criminalIntent, params ILegalPerson[] persons)
        {
            return true;
        }

        /// <summary>
        /// Tests that <see cref="Consent"/> was not given by <see cref="SubjectOfTheft"/> owner
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        protected virtual bool IsWithoutConsent(ILegalPerson[] persons)
        {
            //is all the dependencies present
            if (SubjectOfTheft?.BelongsTo == null || Consent == null 
                                                  || persons == null 
                                                  || !persons.Any())
                return true;

            //did the caller pass in any IVictim types
            var victims = persons.Where(lp => lp is IVictim).ToList();
            if (!victims.Any())
                return true;

            //is any of our victims also the owner of the property
            var ownerVictims = victims.Where(v => VocaBase.Equals(v, SubjectOfTheft.BelongsTo)).ToList();
            if (!ownerVictims.Any())
                return true;

            foreach (var ownerVictim in ownerVictims)
            {
                //did the owner victim in fact give consent 
                if (!Consent.IsDenialExpressed(ownerVictim))
                {
                    AddReasonEntry($"owner-victim {ownerVictim.Name}, {nameof(Consent.IsDenialExpressed)} " +
                                   $"is false for property {SubjectOfTheft}");
                    return false;
                }
            }

            return true;
        }
    }
}
