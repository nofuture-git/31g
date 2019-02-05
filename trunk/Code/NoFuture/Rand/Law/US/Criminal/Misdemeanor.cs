using System;

namespace NoFuture.Rand.Law.US.Criminal
{
    public class Misdemeanor: Infraction
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }

        public override bool IsEnforceableInCourt => true;
    }
}
