using System;
using NoFuture.Rand.Law.Attributes;

namespace NoFuture.Rand.Law.US.Criminal
{
    [Aka("violation")]
    public class Infraction: ObjectiveLegalConcept, ICrime
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }

        public override bool IsEnforceableInCourt => true;
    }
}
