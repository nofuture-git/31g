using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.IntentionalTort
{
    /// <summary>
    /// Legal concept of unjust gain at the expense of another 
    /// </summary>
    /// <remarks>Charrier v. Bell, 496 So. 2d 601 (La. App. 1 Cir. 1986) </remarks>
    [EtymologyNote("Latin","de in rem verso", "of in business back-hand page")]
    public class UnjustEnrichment : UnoHomine
    {
        public UnjustEnrichment(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }
        /// <summary>
        /// There must be impoverishment
        /// </summary>
        public Predicate<IPlaintiff> IsImpoverished { get; set; } = lp => false;

        /// <summary>
        /// There must be enrichment
        /// </summary>
        public Predicate<ILegalPerson> IsEnriched { get; set; } = lp => false;

        /// <summary>
        /// The enrichment-impoverishment reciprocal cause of each to the other
        /// </summary>
        public IProximateCause<ILegalPerson> LegalCause { get; set; }

        /// <summary>
        /// There must be no other remedy at law available to plaintiff
        /// </summary>
        public Predicate<IPlaintiff> IsOtherwiseWithoutRemedyAtLaw { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subj.GetLegalPersonTypeName();

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;
            var pTitle = plaintiff.GetLegalPersonTypeName();

            if (!IsEnriched(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsEnriched)} is false");
                return false;
            }

            if (!IsImpoverished(plaintiff))
            {
                AddReasonEntry($"{pTitle} {plaintiff.Name}, {nameof(IsImpoverished)} is false");
                return false;
            }

            if (!IsOtherwiseWithoutRemedyAtLaw(plaintiff))
            {
                AddReasonEntry($"{pTitle} {plaintiff.Name}, {nameof(IsOtherwiseWithoutRemedyAtLaw)} is false");
                return false;
            }

            if (LegalCause == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(LegalCause)} is unassigned");
                return false;
            }

            if (!LegalCause.IsValid(persons))
            {
                AddReasonEntryRange(LegalCause.GetReasonEntries());
                return false;
            }

            return true;
        }
    }
}
