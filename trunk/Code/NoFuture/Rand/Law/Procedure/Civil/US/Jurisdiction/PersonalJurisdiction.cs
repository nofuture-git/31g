using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Civil.US.Jurisdiction
{
    /// <summary>
    /// The power over person(s) to have them come to a state to be sued in court.
    /// </summary>
    /// <remarks>
    /// <![CDATA[ Since, Pennoyer v. Neff, 95 U.S. 714 (1877), you cannot sue someone in just any jurisdiction ]]>
    /// </remarks>
    public class PersonalJurisdiction : UnoHomine
    {
        public PersonalJurisdiction(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson) { }

        public PersonalJurisdiction() : base(ExtensionMethods.DefendantFx) { }

        /// <summary>
        /// Is where the defendant physically lives or resides - all plaintiffs can
        /// travel there and sue the defendant.
        /// </summary>
        public Predicate<ILegalPerson> IsDomicile { get; set; } = lp => false;

        /// <summary>
        /// If defendant is actually present in the forum state then they may be sued there.
        /// </summary>
        public Predicate<ILegalPerson> IsPhysicallyPresent { get; set; } = lp => false;

        /// <summary>
        /// The defendant agrees to it
        /// </summary>
        public IConsent Consent { get; set; }

        /// <summary>
        /// Defendant meets the minimum contact with the forum state
        /// </summary>
        public MinimumContact MinimumContact { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            var title = defendant.GetLegalPersonTypeName();

            if (IsDomicile(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsDomicile)} is true");
                return true;
            }

            if (IsPhysicallyPresent(defendant))
            {
                AddReasonEntry($"{title} {defendant.Name}, {nameof(IsPhysicallyPresent)} is true");
                return true;
            }

            if (Consent != null && Consent.IsValid(persons))
            {
                AddReasonEntryRange(Consent.GetReasonEntries());
                return true;
            }

            if (MinimumContact != null && MinimumContact.IsValid(persons))
            {
                AddReasonEntryRange(MinimumContact.GetReasonEntries());
                return true;
            }

            return false;
        }

    }
}
