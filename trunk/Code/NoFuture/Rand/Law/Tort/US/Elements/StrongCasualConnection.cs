using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// The use of factual cause when dealing with probability
    /// </summary>
    /// <remarks>
    /// Zuchowicz v. United States, 140 F.3d 381 (2d Cir. 1998)
    /// </remarks>
    public class StrongCasualConnection : UnoHomine, IFactualCause<ILegalPerson>
    {
        public StrongCasualConnection(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// (a) a negligent act was deemed wrongful because that act increased the chances that a particular type of accident would occur
        /// </summary>
        public Predicate<ILegalPerson> IsIncreasedChancesOfEffect { get; set; } = lp => false;

        /// <summary>
        /// (b) a mishap of that very sort did happen
        /// </summary>
        public Predicate<ILegalPerson> IsEffectIndeedPresent { get; set; } = lp => false;

        /// <summary>
        /// this was enough to support a finding by the trier of fact that the negligent behavior caused the harm.
        /// </summary>
        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
            {
                AddReasonEntry($"{nameof(GetSubjectPerson)} returned nothing");
                return false;
            }
            var title = subj.GetLegalPersonTypeName();

            if (!IsIncreasedChancesOfEffect(subj))
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsIncreasedChancesOfEffect)} is false");
                return false;
            }

            var plaintiff = this.Plaintiff(persons);
            if (plaintiff == null)
                return false;

            if (!IsEffectIndeedPresent(plaintiff))
            {
                AddReasonEntry($"{plaintiff.GetLegalPersonTypeName()} {plaintiff.Name}, {nameof(IsEffectIndeedPresent)} is false");
                return false;
            }

            return true;
        }
    }
}
