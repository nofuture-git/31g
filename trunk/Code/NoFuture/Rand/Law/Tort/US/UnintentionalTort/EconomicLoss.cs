using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.UnintentionalTort
{
    /// <summary>
    /// Economic loss requires definite foreseeability concerns who, where, what, etc.
    /// </summary>
    public class EconomicLoss : UnoHomine, INegligence
    {

        public EconomicLoss(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<IPlaintiff> IsEntityTypeIdentifiable { get; set; } = lp => false;

        public Predicate<IPlaintiff> IsEconActivityIdentifiable { get; set; } = t => false;

        public Predicate<IPlaintiff> IsLocationOfEntityPredictable { get; set; } = lp => false;

        public Predicate<IPlaintiff> IsCountOfEntityPredictable { get; set; } = lp => false;

        /// <summary>
        /// The connection of a person to the cause in both fact and law.
        /// </summary>
        public Causation Causation { get; set; }

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

            if (Causation == null)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(Causation)} is unassigned");
                return false;
            }

            if (!Causation.IsValid(persons))
            {
                AddReasonEntryRange(Causation.GetReasonEntries());
                return false;
            }

            title = plaintiff.GetLegalPersonTypeName();

            if (!IsEntityTypeIdentifiable(plaintiff))
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(IsEntityTypeIdentifiable)} is false");
                return false;
            }

            if (!IsEconActivityIdentifiable(plaintiff))
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(IsEconActivityIdentifiable)} is false");
                return false;
            }

            if (!IsLocationOfEntityPredictable(plaintiff))
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(IsLocationOfEntityPredictable)} is false");
                return false;
            }

            if (!IsCountOfEntityPredictable(plaintiff))
            {
                AddReasonEntry($"{title} {plaintiff.Name}, {nameof(IsCountOfEntityPredictable)} is false");
                return false;
            }

            return true;
        }
    }
}
