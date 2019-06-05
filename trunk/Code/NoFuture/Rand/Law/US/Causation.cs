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
        [Aka("loss causation", "causal connection")]
        public IFactualCause FactualCause { get; set; }

        /// <summary>
        /// A reasonable person could have foreseen the outcome
        /// </summary>
        [Aka("legal cause")]
        public Predicate<ILegalPerson> IsForeseeable { get; set; } = lp => false;

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();
            var isForesee = IsForeseeable(defendant);

            if (FactualCause == null)
            {
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(FactualCause)} is unassigned");
                return false;
            }
            
            if (!isForesee)
            {
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(IsForeseeable)} is false");
                return false;
            }

            if (!FactualCause.IsValid(persons))
            {
                AddReasonEntryRange(FactualCause.GetReasonEntries());
                return false;
            }

            return true;
        }
    }
}
