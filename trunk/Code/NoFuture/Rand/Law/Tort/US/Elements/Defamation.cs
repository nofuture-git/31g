using System;
using NoFuture.Rand.Law.Attributes;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// An injury to one&apos;s social standing.
    /// </summary>
    [Aka("calumny", "vilification", "slander (when spoken)", "libel")]
    public class Defamation : UnoHomine, ITermCategory, IInjury
    {
        private readonly ITermCategory _termCategory = new TermCategory(nameof(Harm));

        public Defamation(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// The statement must be false
        /// </summary>
        public Func<ILegalPerson, ILegalPerson, bool> IsFalseStatement { get; set; }

        /// <summary>
        /// The statement must be communicated to others besides the two adversaries
        /// </summary>
        [Eg("spoken to crowd", "printed", "broadcast")]
        public Predicate<ILegalPerson> IsPublishedStatement { get; set; }

        /// <summary>
        /// The statement must be unwanted by the defamed person
        /// </summary>
        public Predicate<ILegalPerson> IsUnwantedStatement { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var subjTitle = subj.GetLegalPersonTypeName();

            var defamed = this.Plaintiff(persons) as ILegalPerson ?? this.Victim(persons);
            if (defamed == null)
            {
                return false;
            }

            var isFalse = IsFalseStatement(subj, defamed) || IsFalseStatement(defamed, subj);

            if (!isFalse)
            {
                AddReasonEntry($"{subjTitle} {subj.Name}, {nameof(IsFalseStatement)} about " +
                               $"{defamed.GetLegalPersonTypeName()} {defamed.Name} is, in fact, true");
                return false;
            }

            if (!IsPublishedStatement(subj))
            {
                AddReasonEntry($"{subjTitle} {subj.Name}, {nameof(IsPublishedStatement)} is false");
                return false;
            }

            if (!IsUnwantedStatement(defamed))
            {
                AddReasonEntry($"{defamed.GetLegalPersonTypeName()} {defamed.Name}, {nameof(IsUnwantedStatement)} is false");
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
