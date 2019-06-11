using System;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.UnintentionalTort
{
    /// <summary>
    /// Negligence by the explicit standard laid down by the Legislature
    /// </summary>
    public class NegligenceByStatute : UnoHomine, INegligence
    {
        public NegligenceByStatute(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// The Restatement (Second) of Torts advises that using a statue to prove negligence
        /// then all the parties in the suit must be members of the group or class of people
        /// for which the statue intended to protect.
        /// </summary>
        public Predicate<ILegalPerson> IsGroupMemberOfStatuesProtection { get; set; } = lp => true;

        /// <summary>
        /// Where the objective tests are directly given by Legislature
        /// </summary>
        public Predicate<ILegalPerson> IsObeyStatute { get; set; } = lp => false;

        /// <summary>
        /// rare case where obedience to statute would increase very danger it intended to reduce
        /// </summary>
        public Predicate<ILegalPerson> IsObedienceAddToDanger { get; set; } = lp => false;

        /// <summary>
        /// <![CDATA[
        /// while compliance with a statute may constitute some evidence of due care, it does
        /// not preclude a finding of negligence.” 539 N.Y.S.2d 922, 924 (1989),
        /// aff’d, 552 N.E.2d 162 (1990).
        /// ]]>
        /// </summary>
        public Predicate<ILegalPerson> IsObedienceSufficient { get; set; } = lp => true;

        /// <summary>
        /// Idea that private parties can sue based on negligence of a
        /// statute - Legislature may intend to directly avoid this.
        /// </summary>
        public Predicate<ILegalPerson> IsDisobedienceCauseForAction { get; set; } = lp => true;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            foreach (var p in persons)
            {
                if (!IsGroupMemberOfStatuesProtection(p))
                {
                    AddReasonEntry($"{title} {p.Name}, {nameof(IsGroupMemberOfStatuesProtection)} is false");
                    return false;
                }

                if (!IsDisobedienceCauseForAction(p))
                {
                    AddReasonEntry($"{title} {p.Name}, {nameof(IsDisobedienceCauseForAction)} is false");
                    return false;
                }
            }

            var obey = IsObeyStatute(subj);
            var disobeyOk = IsObedienceAddToDanger(subj);

            if (!obey && disobeyOk)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsObeyStatute)} is false");
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsObedienceAddToDanger)} is true");
                return false;
            }

            var obeyEnough = IsObedienceSufficient(subj);

            if (obey && obeyEnough)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsObedienceSufficient)} is true");
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsObeyStatute)} is true");
                return false;
            }

            return true;
        }
    }
}
