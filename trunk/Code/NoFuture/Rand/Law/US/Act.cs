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

        public static IAct DueDiligence() { return new Yes();}

        private class Yes : Act
        {
            public override bool IsValid(params ILegalPerson[] persons)
            {
                return true;
            }
        }

        public Predicate<ILegalPerson> IsVoluntary { get; set; } = lp => false;
        public Predicate<ILegalPerson> IsAction { get; set; } = lp => false;

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

            var isAction = IsAction(defendant);

            if (!isAction)
            {
                AddReasonEntry($"the {title} {defendant.Name}, {nameof(IsAction)} is false");
                return false;
            }

            return true;
        }

    }
}
