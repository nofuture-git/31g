using System;

namespace NoFuture.Rand.Law.US
{
    /// <summary>
    /// The idea of some <see cref="ILegalProperty"/> being impaired in value, usefulness or function
    /// </summary>
    public class Damage : UnoHomine
    {
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

            var diminished = ToValue(SubjectProperty)
                             || ToUsefulness(SubjectProperty)
                             || ToNormalFunction(SubjectProperty)
                             ;
            if (!diminished)
            {
                AddReasonEntry($"defendant, {subjectPerson.Name}, {nameof(ToValue)}, " +
                    $"{nameof(ToUsefulness)}, {nameof(ToNormalFunction)} are all false");
            }

            return diminished;
        }
    }
}
