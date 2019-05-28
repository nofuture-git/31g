using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.Elements.ReasonableCare
{
    public class OfExpert<T> : ReasonableCareBase where T: ILegalConcept
    {
        public OfExpert(Func<ILegalPerson[], ILegalPerson> getSubjectPerson)
        {
            GetSubjectPerson = getSubjectPerson;
        }

        public OfExpert() : this(ExtensionMethods.Expert<ILegalConcept>) { }

        public Predicate<ILegalPerson> IsSignificantDanger { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsReliedUpon { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;

            var title = subj.GetLegalPersonTypeName();

            var isExpert = subj is IExpert<T>;

            if (!isExpert)
            {
                AddReasonEntry($"{title} {subj.Name}, is not an {nameof(IExpert<T>)}");
                return base.IsValid(persons);
            }

            var significant = IsSignificantDanger(subj);
            var reliedOn = IsReliedUpon(subj);

            if (!significant && !reliedOn)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsSignificantDanger)} is false");
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsReliedUpon)} is false");
                return false;
            }

            return base.IsValid(persons);
        }
    }
}
