using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// Injury to one&apos;s expectation of privacy
    /// </summary>
    public class InvasionOfPrivacy : UnoHomine, ITermCategory, IInjury
    {
        private readonly ITermCategory _termCategory = new TermCategory(nameof(Harm));

        public InvasionOfPrivacy(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// When given in plain sight of all then privacy cannot be expected
        /// </summary>
        public Predicate<ILegalPerson> IsInPlainSight { get; set; } = lp => false;

        /// <summary>
        /// Would a reasonable person expect privacy in the given circumstances
        /// </summary>
        [Eg("home", "hotel room", "restrooms")]
        public Predicate<ILegalPerson> IsExpectationOfPrivacy { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            if (IsInPlainSight(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsInPlainSight)} is true");
                return false;
            }

            if (!IsExpectationOfPrivacy(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsExpectationOfPrivacy)} is false");
                return false;
            }

            return true;
        }

        public virtual int GetRank()
        {
            return _termCategory.GetRank();
        }

        public virtual string GetCategory()
        {
            return _termCategory.GetCategory();
        }

        public virtual bool IsCategory(ITermCategory category)
        {
            return _termCategory.IsCategory(category);
        }

        public virtual ITermCategory As(ITermCategory category)
        {
            return _termCategory.As(category);
        }

        public virtual bool IsCategory(Type category)
        {
            return _termCategory.IsCategory(category);
        }
    }
}
