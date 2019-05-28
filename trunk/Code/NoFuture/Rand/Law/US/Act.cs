using System;

namespace NoFuture.Rand.Law.US
{
    /// <inheritdoc cref="IAct"/>
    public class Act : UnoHomine, IAct
    {
        public Act(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            
        }
        public Act() : this(ExtensionMethods.Defendant) { }

        public Predicate<ILegalPerson> IsVoluntary { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsAction { get; set; } = lp => false;
        public virtual Duty Duty { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();

            if (!IsVoluntary(defendant))
            {
                AddReasonEntry($"the {title} {defendant.Name}, {nameof(IsVoluntary)} is false");
                return false;
            }

            //test for lack-of-action first, if its expected
            if (Duty != null)
            {
                var didAct = IsAction(defendant);
                var duty = Duty.IsValid(persons) && didAct;
                AddReasonEntry($"the {title} {defendant.Name}, {nameof(IsAction)} is {didAct}");
                AddReasonEntryRange(Duty.GetReasonEntries());
                return duty;
            }

            if (!IsAction(defendant))
            {
                AddReasonEntry($"the {title} {defendant.Name}, {nameof(IsAction)} is false");
                return false;
            }

            return true;
        }

    }
}
