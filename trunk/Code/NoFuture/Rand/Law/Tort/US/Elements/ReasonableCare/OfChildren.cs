using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements.ReasonableCare
{
    /// <summary>
    /// What is reasonable is based on age unless a duty otherwise.
    /// </summary>
    public class OfChildren : ReasonableCareBase
    {
        public OfChildren(Func<ILegalPerson[], ILegalPerson> getSubjectPerson)
        {
            GetSubjectPerson = getSubjectPerson;
        }

        public OfChildren() : this (ExtensionMethods.Tortfeasor) { }

        /// <summary>
        /// child not being held to the same standard of conduct as an adult
        /// and being required to exercise only that degree of care [...] by
        /// children of like age, mental capacity, and experience under the
        /// same or similar circumstances
        /// </summary>
        /// <remarks>
        /// src: Dellwo v. Pearson, 107 N.W.2d 859 (Minn. 1961)
        /// </remarks>
        public Predicate<ILegalPerson> IsUnderage { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            var isChild = IsUnderage(subj);
            var isAction = IsAction(subj);
            var isVoluntary = IsVoluntary(subj);
            var isDangerousAdultActivity = Duty != null && Duty.IsValid(persons);

            if (!isVoluntary)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsVoluntary)} is false");
                return false;
            }

            if (isDangerousAdultActivity && !isAction)
            {
                AddReasonEntryRange(Duty.GetReasonEntries());
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsAction)} required by {nameof(Duty)} is false");
                return false;
            }

            if (!isChild)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsUnderage)} is false");
                return false;
            }

            return true;
        }
    }
}
