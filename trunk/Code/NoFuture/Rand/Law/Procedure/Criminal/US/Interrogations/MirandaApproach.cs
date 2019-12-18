using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Interrogations
{
    /// <summary>
    /// The familiar warnings given to a person by law enforcement prior to interrogation 
    /// </summary>
    /// <remarks>
    /// questioning initiated by law enforcement officers after a person has
    /// taken into custody or otherwise deprived of his freedom of action
    /// in any significant way 384 U.S. at 443
    /// </remarks>
    [Aka("custodial interrogation")]
    public class MirandaApproach : LegalConcept
    {
        public Func<ILegalPerson[], ILegalPerson> GetSuspect { get; set; } = lps => lps.Suspect();

        public Predicate<ILegalPerson> IsToldRightToRemainSilent { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsToldRightToAttorney { get; set; } = lp => false;

        [Aka("waiver of Miranda Rights")]
        public IConsent Consent { get; set; }

        /// <summary>
        /// A test of whether a reasonable person in the
        /// situation would have believed they are not free to leave
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        /// Other factors for analysis:
        /// * was informed as free-to-leave
        /// * unrestrained freedom of movement
        /// * initiated contact
        /// * voluntarily acquiesced
        /// * use of police strong-arm tatics
        /// * use of police deceptive strategies
        /// * atmosphere is police-dominated
        /// ]]>
        /// </remarks>
        [Aka("custody", "police-dominated atmosphere")]
        public Predicate<ILegalPerson> IsInCoercivePressureEnvironment { get; set; } = lp => false;

        /// <summary>
        /// encompasses conduct deliberately designed to evoke a confession, as well as
        /// conduct that the officers should reasonably have foreseen would elicit such a response
        /// </summary>
        public Predicate<ILegalPerson> IsIncriminatoryQuestioning { get; set; } = lp => false;

        /// <summary>
        /// New York v. Quarles, 467 U.S. 649 (1984) when overriding considerations of
        /// public safety justify the officer&apos;s failure to provide Miranda warnings
        /// </summary>
        public Predicate<ILegalPerson> IsPublicSafetyException { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (Consent != null && Consent.IsValid(persons))
            {
                AddReasonEntryRange(Consent?.GetReasonEntries());
                return true;
            }

            var suspect = this.Suspect(persons, GetSuspect);
            if (suspect == null)
                return false;

            var suspectTitle = suspect.GetLegalPersonTypeName();

            if (!IsInCoercivePressureEnvironment(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsInCoercivePressureEnvironment)} is false");
                return false;
            }

            if (!IsIncriminatoryQuestioning(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsIncriminatoryQuestioning)} is false");
                return false;
            }

            if (IsPublicSafetyException(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsPublicSafetyException)} is false");
                return false;
            }

            if (!IsToldRightToAttorney(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsToldRightToAttorney)} is false");
                return false;
            }

            if (!IsToldRightToRemainSilent(suspect))
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(IsToldRightToRemainSilent)} is false");
                return false;
            }

            return true;
        }
    }
}
