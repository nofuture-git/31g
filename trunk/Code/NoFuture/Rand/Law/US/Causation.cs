using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US
{
    public class Causation : UnoHomine
    {
        public Causation(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        /// <summary>
        /// The direct antecedent which caused the harm\damage - the harm\damage which exist only because of it.
        /// </summary>
        [Aka("factual cause")]
        public Predicate<ILegalPerson> IsButForCaused { get; set; } = lp => false;

        /// <summary>
        /// A reasonable person could have foreseen the outcome
        /// </summary>
        [Aka("legal cause")]
        public ObjectivePredicate<ILegalPerson> IsForeseeable { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var isButFor = IsButForCaused(defendant);
            var isForesee = IsForeseeable(defendant);

            if (!isButFor)
            {
                
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(IsButForCaused)} is false");
                return false;
            }

            if (!isForesee)
            {
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(IsForeseeable)} is false");
                return false;
            }

            return true;
        }
    }
}
