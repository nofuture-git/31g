using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US
{
    /// <inheritdoc cref="IFactualCause{T}" />
    public class FactualCause : UnoHomine, IFactualCause<ILegalPerson>
    {
        public FactualCause(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        [EtymologyNote("latin", "sine qua non", "without which nothing")]
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
