using System.Collections.Generic;
using NoFuture.Rand.Law.US.Criminal.Elements;

namespace NoFuture.Rand.Law.US.Criminal
{
    public abstract class CrimeBase : ObjectiveLegalConcept, ICrime
    {
        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            offeree = offeree ?? Government.Value;
            offeror = offeror ?? Government.Value;
            ILegalPerson defendant = null;
            if (!offeror.Equals(Government.Value) && offeree.Equals(Government.Value))
                defendant = offeror;
            if (offeror.Equals(Government.Value) && !offeree.Equals(Government.Value))
                defendant = offeree;


            if (defendant == null)
            {
                AddReasonEntry($"it is not clear who the defendant is between {offeror.Name} and {offeree.Name}");
            }

            if (!Concurrence.IsValid(offeror, offeree))
            {
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

        public Concurrence Concurrence => new Concurrence();
        public IList<IElement> AdditionalElements => new List<IElement>();
    }
}
