using System;
using NoFuture.Rand.Law.Tort.US.Terms;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// liabilities in tort arise from failure to comply with fixed and
    /// uniform standards of external conduct, which every man is presumed
    /// and required to know
    /// [OLIVER WENDELL HOLMES, JR., THE COMMON LAW 111, 123-4 (1881)]
    /// </summary>
    public class Negligence : UnoHomine
    {
        public Negligence(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// defendant&apos;s conduct was morally wrong according to prevailing community norms
        /// </summary>
        public Predicate<ILegalPerson> IsConductMorallyWrong { get; set; } = lp => false;

        /// <summary>
        /// individuals in the jury, upon placing themselves in the defendant&apos;s
        /// shoes along with prudence and carefulness, would have acted as the
        /// defendant actually did
        /// </summary>
        public Predicate<ILegalPerson> IsWithoutReasonablePrudence { get; set; } = lp => false;

        /// <summary>
        /// whether the defendant breached a safety convention commonly understood
        /// in the community to protect the kinds of people like the plaintiff
        /// </summary>
        public CustomsTerm SafetyConvention { get; set; }

        /// <summary>
        /// whether an ordinary, reasonable person would have foreseen
        /// danger to others under known circumstances
        /// </summary>
        public Predicate<ILegalPerson> IsForeseeableDangerToOthers { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            if (IsConductMorallyWrong(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsConductMorallyWrong)} is true");
                return true;
            }

            if (IsWithoutReasonablePrudence(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsWithoutReasonablePrudence)} is true");
                return true;
            }

            if (SafetyConvention != null && SafetyConvention.IsValid(persons) == false)
            {
                AddReasonEntryRange(SafetyConvention.GetReasonEntries());
                AddReasonEntry($"{title} {subj.Name}, {nameof(SafetyConvention)} {nameof(IsValid)} is false");
                return true;
            }

            if (IsForeseeableDangerToOthers(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsForeseeableDangerToOthers)} is true");
                return true;
            }

            return false;
        }
    }
}
