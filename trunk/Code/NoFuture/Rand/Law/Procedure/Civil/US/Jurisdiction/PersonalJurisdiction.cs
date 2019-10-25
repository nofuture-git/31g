using System;
using System.Linq;
using NoFuture.Rand.Core;
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
        public PersonalJurisdiction() { }
        public PersonalJurisdiction(string name) :base(name) { }


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
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;

            var title = defendant.GetLegalPersonTypeName();
            foreach(var domicile in GetDomicileLocation(defendant) ?? new IVoca[] { })
            {
                if (domicile != null && NameEquals(domicile))
                {
                    AddReasonEntry($"{title} {defendant.Name}, {nameof(GetDomicileLocation)} " +
                                   $"returned a name whose {nameof(NameEquals)} is true for '{Name}'");
                    return true;
                }
            }

            foreach (var location in GetPhysicalLocation(defendant) ?? new IVoca[] { })
            {
                if (location != null && NameEquals(location))
                {
                    AddReasonEntry($"{title} {defendant.Name}, {nameof(GetPhysicalLocation)} " +
                                   $"returned a name whose {nameof(NameEquals)} is true for '{Name}'");
                    return true;
                }
            }

            if (Consent != null && Consent.IsValid(persons))
            {
                AddReasonEntryRange(Consent.GetReasonEntries());
                return true;
            }

            if (MinimumContact != null)
            {
                //transpose whatever is here to this sister type based on what's missing
                if(MinimumContact.NamesCount <= 0)
                    MinimumContact.CopyNamesFrom(this);

                if (flagGetDomicileLocation && MinimumContact.flagGetDomicileLocation == false)
                    MinimumContact.GetDomicileLocation = GetDomicileLocation;

                if (flagGetPhysicalLocation && MinimumContact.flagGetPhysicalLocation == false)
                    MinimumContact.GetPhysicalLocation = GetPhysicalLocation;

                if (MinimumContact.IsValid(persons))
                {
                    AddReasonEntryRange(MinimumContact.GetReasonEntries());
                    return true;
                }

            }

            return false;
        }

    }
}
