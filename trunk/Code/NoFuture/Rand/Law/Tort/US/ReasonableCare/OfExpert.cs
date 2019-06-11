using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.ReasonableCare
{
    /// <summary>
    /// A higher standard of reasonable care, beyond fortuity, when one is an expert of said thing.
    /// </summary>
    /// <typeparam name="T"> An expert of what </typeparam>
    public class OfExpert<T> : ReasonableCareBase where T: IRationale
    {
        public OfExpert(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// One of two reasons, &quot;beyond fortuity&quot; the reasonable care standard is raised.
        /// </summary>
        public Predicate<ILegalPerson> IsSignificantDanger { get; set; } = lp => false;

        /// <summary>
        /// One of two reasons, &quot;beyond fortuity&quot; the reasonable care standard is raised.
        /// </summary>
        public Predicate<ILegalPerson> IsReliedUpon { get; set; } = lp => false;

        public Predicate<ILegalPerson> IsExercisedExpertCare { get; set; } = lp => false;

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
                return false;
            }

            var significant = IsSignificantDanger(subj);
            var reliedOn = IsReliedUpon(subj);

            if (!significant && !reliedOn)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsSignificantDanger)} is false");
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsReliedUpon)} is false");
                return false;
            }

            if (!IsExercisedExpertCare(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsExercisedExpertCare)} is false");
                return false;
            }

            return true;
        }
    }
}
