using System;
using NoFuture.Law.US;
using NoFuture.Law.US.Persons;

namespace NoFuture.Law.Tort.US.ReasonableCare
{
    /// <summary>
    /// What is reasonable is based on age unless a duty otherwise.
    /// </summary>
    public class OfChildren : ReasonableCareBase
    {
        public OfChildren(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// child not being held to the same standard of conduct as an adult
        /// and being required to exercise only that degree of care [...] by
        /// children of like age, mental capacity, and experience under the
        /// same or similar circumstances
        /// </summary>
        /// <remarks>
        /// src: Dellwo v. Pearson, 107 N.W.2d 859 (Minn. 1961)
        /// </remarks>
        public Predicate<ILegalPerson> IsExercisedAdultCare { get; set; } = lp => false;

        public Predicate<IAct> IsDangerousAdultActivity { get; set; } = lp => false;

        public Func<ILegalPerson, IAct> GetActionOfPerson { get; set; } = lp => null;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subj.GetLegalPersonTypeName();
            Predicate<ILegalPerson> isUnderage = lp => lp is IChild;
            if (!isUnderage(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, is {nameof(IChild)} is false");
                return false;
            }

            var act = GetActionOfPerson(subj);

            if (!IsDangerousAdultActivity(act))
            {
                return true;
            }
            AddReasonEntry($"{title} {subj.Name}, {nameof(IsDangerousAdultActivity)} for {act?.GetType().Name} is true");
            if ( !IsExercisedAdultCare(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsExercisedAdultCare)} is false");
                return false;
            }

            return true;
        }
    }
}
