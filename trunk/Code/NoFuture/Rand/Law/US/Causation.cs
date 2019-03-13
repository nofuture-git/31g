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
        /// The direct antecedent which caused the harm - the harm which exist only because of it.
        /// </summary>
        [Aka("factual cause")]
        public Predicate<ILegalPerson> IsButForCaused { get; set; } = lp => false;

        /// <summary>
        /// A reasonable person could have foreseen the outcome
        /// </summary>
        [Aka("legal cause")]
        public ObjectivePredicate<ILegalPerson> IsForseeable { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;

            var isButFor = IsButForCaused(defendant);
            var isForesee = IsForseeable(defendant);

            if (!isButFor)
            {
                
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsButForCaused)} is false");
                return false;
            }

            if (!isForesee)
            {
                AddReasonEntry($"defendant, {defendant.Name}, {nameof(IsForseeable)} is false");
                return false;
            }

            return true;
        }
    }
}
