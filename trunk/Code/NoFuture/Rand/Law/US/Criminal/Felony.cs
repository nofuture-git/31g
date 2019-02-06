using System;

namespace NoFuture.Rand.Law.US.Criminal
{
    public class Felony : Misdemeanor
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            throw new NotImplementedException();
        }
        public override int CompareTo(object obj)
        {
            if (obj is Misdemeanor || obj is Infraction)
                return 1;
            return 0;
        }

        public override bool IsEnforceableInCourt => true;
    }
}
