using System;
using NoFuture.Law.Attributes;
using NoFuture.Law.US;

namespace NoFuture.Law.Procedure.Criminal.US.Interrogations
{
    /// <summary>
    /// Prohibits the police from deliberately eliciting incriminating
    /// statements from an accused in the absence of counsel after the
    /// right to counsel has attached.
    /// Massiah v. United States, 377 U.S. 201 (1964)
    /// </summary>
    public class RightToCounselApproach : CommencementJudicialProceedings
    {
        public Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; } = lps => lps.LawEnforcement();

        /// <summary>
        /// the government deliberately elicited incriminating statements from the accused in the absence of counsel (or waiver of counsel)
        /// </summary>
        public Predicate<ILegalPerson> IsDeliberateElicit { get; set; } = lp => false;

        [Aka("waiver of Miranda Rights")]
        public IConsent Consent { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (Consent != null && Consent.IsValid(persons))
            {
                AddReasonEntryRange(Consent?.GetReasonEntries());
                return true;
            }

            var officer = GetLawEnforcement(persons);
            if (officer == null)
            {
                AddReasonEntry($"{nameof(GetLawEnforcement)} returned nothing");
                return false;
            }

            var officerTitle = officer.GetLegalPersonTypeName();

            if (!IsDeliberateElicit(officer))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsDeliberateElicit)} is false");
                return false;
            }

            return IsJudicialProceedingsInitiated(persons);
        }
    }
}
