using System;
using System.Linq;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// Jurisdiction in terms of its geographical limitations
    /// </summary>
    /// <remarks>
    /// <![CDATA[ Since, Pennoyer v. Neff, 95 U.S. 714 (1877), you cannot sue someone in just any jurisdiction ]]>
    /// </remarks>
    public class PersonalJurisdiction : JurisdictionBase
    {
        public PersonalJurisdiction(ICourt name) :base(name) { }

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

            //is consented to jurisdiction
            if (Consent != null && Consent.IsValid(persons))
            {
                AddReasonEntryRange(Consent.GetReasonEntries());
                if (!GetReasonEntries().Any())
                {
                    AddReasonEntry($"{title} {defendant.Name}, {nameof(Consent)} {nameof(IsValid)} returned true");
                }
                return true;
            }

            //is pass minimum contact test
            if (IsMinimumContact(persons))
            {
                return true;
            }
            //idea of general in personam jurisdiction
            if (IsCourtDomicileLocationOfDefendant(defendant, title))
            {
                return true;
            }

            if (IsCourtCurrentLocationOfDefendant(defendant, title))
            {
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
