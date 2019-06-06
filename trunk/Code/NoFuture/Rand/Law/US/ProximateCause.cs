using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US
{
    [Aka("legal cause")]
    public class ProximateCause : UnoHomine, IProximateCause
    {
        public ProximateCause(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsDirectCause { get; set; } = lp => false;

        /// <summary>
        /// A reasonable person could have foreseen the outcome
        /// </summary>
        public Predicate<ILegalPerson> IsForeseeable { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            var directCause = IsDirectCause(subj);
            var foreseeable = IsForeseeable(subj);

            if (!directCause && !foreseeable)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsDirectCause)} is false");
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsForeseeable)} is false");
                return false;
            }

            return true;
        }

    }
}
