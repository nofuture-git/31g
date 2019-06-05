using System;

namespace NoFuture.Rand.Law.US
{
    /// <inheritdoc cref="IFactualCause" />
    public class FactualCause : UnoHomine, IFactualCause
    {
        public FactualCause(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Predicate<ILegalPerson> IsButForCaused { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var isButFor = IsButForCaused(defendant);

            if (!isButFor)
            {
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(IsButForCaused)} is false");
                return false;
            }

            return true;
        }
    }
}
