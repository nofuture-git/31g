using System;

namespace NoFuture.Rand.Law.US
{

    public class Duty : UnoHomine, IDuty
    {
        public Duty(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {

        }

        public Duty() : this(ExtensionMethods.Defendant) { }

        /// <summary>
        /// The duty to act originates from a statute
        /// </summary>
        public Predicate<ILegalPerson> IsStatuteOrigin { get; set; } = lp => false;

        /// <summary>
        /// The duty to act originates from a contract
        /// </summary>
        public Predicate<ILegalPerson> IsContractOrigin { get; set; } = lp => false;

        /// <summary>
        /// The duty to act originates from any form of special relationship (e.g. spouse-spouse, parent-child, etc.)
        /// </summary>
        public Predicate<ILegalPerson> IsSpecialRelationshipOrigin { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            if (IsStatuteOrigin(defendant))
            {
                AddReasonEntry($"the {title} {defendant.Name}, {nameof(Duty)} {nameof(IsStatuteOrigin)} is true");
                return true;
            }

            if (IsContractOrigin(defendant))
            {
                AddReasonEntry($"the {title} {defendant.Name}, {nameof(Duty)} {nameof(IsContractOrigin)} is true");
                return true;
            }

            if (IsSpecialRelationshipOrigin(defendant))
            {
                AddReasonEntry($"the {title} {defendant.Name}, {nameof(Duty)} {nameof(IsSpecialRelationshipOrigin)} is true");
                return true;
            }

            return false;
        }
    }
}
