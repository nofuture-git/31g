using System;
using NoFuture.Rand.Law;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Procedure.Criminal.US.Intrusions
{
    /// <summary>
    /// the police are permitted to seize other articles of contraband or evidence of crime that
    /// they come upon in the ordinary course of the original search
    /// </summary>
    /// <remarks>
    /// needs to both obviously apparent and obviously illegal
    /// </remarks>
    public class PlainViewSeizure : LegalConcept, IIntrusion
    {
        public Func<ILegalPerson[], ILegalPerson> GetSuspect { get; set; } = lps => lps.Suspect();
        public Func<ILegalPerson[], ILegalPerson> GetLawEnforcement { get; set; } = lps => lps.LawEnforcement();

        public Func<ILegalPerson, IVoca> GetObjectOfSeizure { get; set; } = lp => null;

        /// <summary>
        /// (1) the original intrusion must be, itself, lawful
        /// </summary>
        public IIntrusion OriginalIntrusion { get; set; }

        /// <summary>
        /// (2) observed while the officer is confining their activities to permissible scope of that intrusion
        /// </summary>
        public Predicate<ILegalPerson> IsObservedInPermissibleScope { get; set; } = lp => false;

        /// <summary>
        /// (3) is immediately apparent that the item is contraband or evidence of crime
        /// </summary>
        public Predicate<IVoca> IsPlainlyApparentClearlyIllegal { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var suspect = this.Suspect(persons, GetSuspect);
            var officer = this.LawEnforcement(persons, GetLawEnforcement);

            if (suspect == null || officer == null)
                return false;

            var officerTitle = officer.GetLegalPersonTypeName();
            var suspectTitle = suspect.GetLegalPersonTypeName();

            var item = GetObjectOfSeizure(suspect);
            if (item == null)
            {
                AddReasonEntry($"{suspectTitle} {suspect.Name}, {nameof(GetObjectOfSeizure)} returned nothing");
                return false;
            }

            if (OriginalIntrusion == null || !OriginalIntrusion.IsValid(persons))
            {
                AddReasonEntry($"{nameof(OriginalIntrusion)} {nameof(IsValid)} is false");
                AddReasonEntryRange(OriginalIntrusion?.GetReasonEntries());
                return false;
            }

            if (!IsObservedInPermissibleScope(officer))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsObservedInPermissibleScope)} is false");
                return false;
            }

            if (!IsPlainlyApparentClearlyIllegal(item))
            {
                AddReasonEntry($"{officerTitle} {officer.Name}, {nameof(IsPlainlyApparentClearlyIllegal)} is false");
                return false;
            }

            return true;
        }

    }
}
