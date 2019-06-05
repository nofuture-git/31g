using System;
using NoFuture.Rand.Law.US;
using NoFuture.Rand.Law.US.Persons;

namespace NoFuture.Rand.Law.Tort.US.Elements
{
    public class ConcertOfAction : UnoHomine
    {
        public ConcertOfAction(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// The third party participated directly in the tortious act
        /// </summary>
        public Func<ILegalPerson, IThirdParty, bool> IsPerformAlongside { get; set; } = (lp, thp) => false;

        /// <summary>
        /// The third party assisted in some way toward the tortious act 
        /// </summary>
        public Func<ILegalPerson, IThirdParty, bool> IsAssistingWith { get; set; } = (lp, thp) => false;

        /// <summary>
        /// The third party was aware of tortfeaser&apos;s breach-of-duty toward plaintiff
        /// or, they themselves, breached their duty toward the plaintiff
        /// </summary>
        public Func<ILegalPerson, IThirdParty, bool> IsAwareBreachOfDuty { get; set; } = (lp, thp) => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var subj = GetSubjectPerson(persons);

            if (subj == null)
                return false;
            var title = subj.GetLegalPersonTypeName();

            var thirdParty = this.ThirdParty(persons);
            if (thirdParty == null)
                return false;
            var thirdPartyTitle = thirdParty.GetLegalPersonTypeName();

            var alongWith = IsPerformAlongside(subj, thirdParty);
            var assisted = IsAssistingWith(subj, thirdParty);
            var breachDuty = IsAwareBreachOfDuty(subj, thirdParty);

            if (!alongWith && !(assisted && breachDuty))
            {
                AddReasonEntry($"{title} {subj.Name} and {thirdPartyTitle} {thirdParty.Name}, " +
                               $"{nameof(IsPerformAlongside)} is false");
                AddReasonEntry($"{title} {subj.Name} and {thirdPartyTitle} {thirdParty.Name}, " +
                               $"{nameof(IsAssistingWith)} and {nameof(IsAwareBreachOfDuty)} are together false");
                return false;
            }

            return true;
        }

    }
}
