using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    public class Negligence : UnoHomine
    {
        public Negligence(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// defendant&apos;s conduct was morally wrong according to prevailing community norms
        /// </summary>
        public Predicate<ILegalPerson> IsConductMorallyWrong { get; set; }

        /// <summary>
        /// individuals in the jury, upon placing themselves in the defendant&apos;s
        /// shoes along with prudence and carefulness, would have acted as the
        /// defendant actually did
        /// </summary>
        public Predicate<ILegalPerson> IsWithoutReasonablePrudence { get; set; }

        /// <summary>
        /// whether the defendant breached a safety convention commonly understood
        /// in the community to protect the kinds of people like the plaintiff
        /// </summary>
        public Predicate<ILegalPerson> IsBreachSafetyConvention { get; set; }

        /// <summary>
        /// whether an ordinary, reasonable person would have foreseen
        /// danger to others under known circumstances
        /// </summary>
        public Predicate<ILegalPerson> IsForeseeableDangerToOthers { get; set; }

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

            if (IsBreachSafetyConvention(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsBreachSafetyConvention)} is true");
                return true;
            }

            if (IsForeseeableDangerToOthers(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsForeseeableDangerToOthers)} is true");
                return true;
            }

            return true;
        }
    }
}
