using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal.Elements
{
    [Aka("criminal act")]
    public class MensRea : ObjectiveLegalConcept, IElement
    {
        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            return true;
        }
    }
}
