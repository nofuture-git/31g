using System;

namespace NoFuture.Rand.Law.US.Criminal
{
    public class Misdemeanor: Infraction
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }

        public override int CompareTo(object obj)
        {
            if (obj is Felony)
                return -1;
            if (obj is Infraction)
                return 1;
            return 0;
        }

        public override bool IsEnforceableInCourt => true;
    }
}
