using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Law.US;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    /// <summary>
    /// Negligence by the explicit standard laid down by the Legislature
    /// </summary>
    public class NegligenceByStatute : UnoHomine
    {
        public NegligenceByStatute(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// Where the objective tests are directly given by Legislature
        /// </summary>
        public Predicate<ILegalPerson> IsObeyStatute { get; set; } = lp => false;

        /// <summary>
        /// rare case where obedience to statute would increase very danger it intended to reduce
        /// </summary>
        public Predicate<ILegalPerson> IsObedienceAddToDanger { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);
            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            var obey = IsObeyStatute(subj);
            var disobeyOk = IsObedienceAddToDanger(subj);

            if (!obey && disobeyOk)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsObeyStatute)} is false");
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsObedienceAddToDanger)} is true");
                return false;
            }

            if (obey)
            {
                AddReasonEntry($"{title} {subj.Name}, {nameof(IsObeyStatute)} is true");
                return false;
            }

            return true;
        }
    }
}
