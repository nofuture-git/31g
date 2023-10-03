using System;

namespace NoFuture.Law.US
{
    /// <summary>
    /// The idea of some <see cref="ILegalProperty"/> being impaired in value, usefulness or function
    /// </summary>
    public class Damage : UnoHomine, ITermCategory, IInjury
    {
        private readonly ITermCategory _termCategory = new TermCategory(nameof(Damage));
        public Damage(): this(ExtensionMethods.Defendant) { }
        public Damage(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public ILegalProperty SubjectProperty { get; set; }

        public Predicate<ILegalProperty> ToValue { get; set; } = lo => false;

        public Predicate<ILegalProperty> ToUsefulness { get; set; } = lo => false;

        public Predicate<ILegalProperty> ToNormalFunction { get; set; } = lo => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (SubjectProperty == null)
                return false;
            var subjectPerson = GetSubjectPerson(persons);
            if (subjectPerson == null)
                return false;
            var title = subjectPerson.GetLegalPersonTypeName();
            var diminished = ToValue(SubjectProperty)
                             || ToUsefulness(SubjectProperty)
                             || ToNormalFunction(SubjectProperty)
                             ;
            if (!diminished)
            {
                AddReasonEntry($"{title}, {subjectPerson.Name}, {nameof(ToValue)}, " +
                    $"{nameof(ToUsefulness)}, {nameof(ToNormalFunction)} are all false");
            }

            return diminished;
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
