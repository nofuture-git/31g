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

        public virtual int CompareTo(object obj)
        {
            if (obj is Felony || obj is Misdemeanor)
                return -1;
            return 0;
        }

        public override bool IsEnforceableInCourt => true;

    }
}
