using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US
{
    public class Causation : UnoHomine
    {
        public Causation(Func<ILegalPerson[], ILegalPerson> getSubjectPerson) : base(getSubjectPerson)
        {
        }

        public Causation() : base(ExtensionMethods.Defendant) { }

        public static Causation ItsObvious() { return new Yes();}

        private class Yes : Causation
        {
            public override bool IsValid(params ILegalPerson[] persons)
            {
                return true;
            }
        }

        /// <summary>
        /// The direct antecedent which caused the harm\damage - the harm\damage which exist only because of it.
        /// </summary>
        [Aka("loss causation", "causal connection")]
        public IFactualCause<ILegalPerson> FactualCause { get; set; }

        [Aka("legal cause")]
        public IProximateCause<ILegalPerson> ProximateCause { get; set; }

        public override bool IsValid(params ILegalPerson[] persons)
        {
            var defendant = GetSubjectPerson(persons);
            if (defendant == null)
                return false;
            var title = defendant.GetLegalPersonTypeName();

            if (FactualCause == null)
            {
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(FactualCause)} is unassigned");
                return false;
            }

            if (ProximateCause == null)
            {
                AddReasonEntry($"{title}, {defendant.Name}, {nameof(ProximateCause)} is unassigned");
                return false;
            }

            if (!ProximateCause.IsValid(persons))
            {
                AddReasonEntryRange(FactualCause.GetReasonEntries());
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
