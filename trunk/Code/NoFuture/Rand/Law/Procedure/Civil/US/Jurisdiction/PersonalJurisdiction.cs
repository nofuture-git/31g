using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// The power over person(s) to have them come to a state to be sued in court.
    /// </summary>
    /// <remarks>
    /// <![CDATA[ Since, Pennoyer v. Neff, 95 U.S. 714 (1877), you cannot sue someone in just any jurisdiction ]]>
    /// </remarks>
    public class PersonalJurisdiction : JurisdictionBase
    {
        public PersonalJurisdiction(ICourt name) :base(name) { }

        /// <summary>
        /// the Legislature must still authorize the court to exercise jurisdiction
        /// </summary>
        /// <remarks>
        /// Where Due-Process clause defines the outer bounds of permissible jurisdictional
        /// power - the legislature may limit but never expand jurisdiction beyond it.
        /// </remarks>
        [Aka("long-arm statutes")]
        public virtual Predicate<ILegalPerson> IsAuthorized2ExerciseJurisdiction { get; set; } = lp => true;

        /// <summary>
        /// The defendant agrees to it
        /// </summary>
        public virtual IConsent Consent { get; set; }

        /// <summary>
        /// Defendant meets the minimum contact with the forum state
        /// </summary>
        public virtual MinimumContact MinimumContact { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = this.Defendant(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();

            if (!IsAuthorized2ExerciseJurisdiction(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsAuthorized2ExerciseJurisdiction)} is false");
                return false;
            }

            if (Consent != null && Consent.IsValid(persons))
            {
                AddReasonEntryRange(Consent.GetReasonEntries());
                if (!GetReasonEntries().Any())
                {
                    AddReasonEntry($"{title} {defendant.Name}, {nameof(Consent)} {nameof(IsValid)} returned true");
                }
                return true;
            }

            if (IsMinimumContact(persons))
            {
                return true;
            }

            var domicile = GetDomicileLocation(defendant);
            if (domicile != null && NamesEqual(Court, domicile))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(GetDomicileLocation)} returned '{domicile.Name}'");
                AddReasonEntry($"'{domicile.Name}' & {nameof(Court)} '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            var location = GetCurrentLocation(defendant);
            if (location != null && NamesEqual(Court, location))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(GetCurrentLocation)} returned '{location.Name}'");
                AddReasonEntry($"'{location.Name}' & {nameof(Court)}  '{Court.Name}', {nameof(NamesEqual)} is true");
                return true;
            }

            return false;
        }

        protected virtual bool IsMinimumContact(ILegalPerson[] persons)
        {
            if (MinimumContact == null) 
                return false;

            CopyTo(MinimumContact);

            var result = MinimumContact.IsValid(persons);
            AddReasonEntryRange(MinimumContact.GetReasonEntries());
            return result;
        }
    }
}
