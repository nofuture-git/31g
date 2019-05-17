using System;

namespace NoFuture.Rand.Law.US
{
    /// <inheritdoc cref="IAct"/>
    public class Act : UnoHomine, IAct
    {
        public Act(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
            Omission = new Duty(getSubjectPerson);
        }
        public Act() : this(ExtensionMethods.Defendant) { }

        public Predicate<ILegalPerson> IsVoluntary { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsAction { get; set; } = lp => false;
        public Duty Omission { get; set; }

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

            if (Omission != null && Omission.IsValid(persons))
            {
                AddReasonEntryRange(Omission.GetReasonEntries());
                return true;
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
