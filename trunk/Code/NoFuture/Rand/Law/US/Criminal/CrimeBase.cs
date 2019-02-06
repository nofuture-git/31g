using System.Collections.Generic;
using NoFuture.Rand.Law.US.Criminal.Elements;

namespace NoFuture.Rand.Law.US.Criminal
{
    public abstract class CrimeBase : ObjectiveLegalConcept, ICrime
    {
        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (!Concurrence.IsValid(offeror, offeree))
            {
                AddReasonEntry($"{nameof(Concurrence)} is invalid");
                AddReasonEntryRange(Concurrence.GetReasonEntries());
                return false;
            }

            foreach (var elem in AdditionalElements)
            {
                if (!elem.IsValid(offeror, offeree))
                {
                    AddReasonEntryRange(elem.GetReasonEntries());
                    return false;
                }
            }

            return true;
        }

        public abstract int CompareTo(object obj);

        public Concurrence Concurrence { get; } = new Concurrence();

        /// <summary>
        /// A short hand property to the same stack-residing variable in <see cref="Concurrence"/>
        /// </summary>
        public ActusReus ActusReus => Concurrence.ActusReus;

        /// <summary>
        /// A short hand property to the same stack-residing variable in <see cref="Concurrence"/>
        /// </summary>
        public MensRea MensRea => Concurrence.MensRea;

        public IList<IElement> AdditionalElements { get; } = new List<IElement>();
    }
}
