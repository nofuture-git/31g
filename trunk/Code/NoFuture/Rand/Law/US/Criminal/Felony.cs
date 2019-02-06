using System;

namespace NoFuture.Rand.Law.US.Criminal
{
    /// <inheritdoc cref="ICrime"/>
    public class Felony : CrimeBase
    {
        public override bool IsValid(ILegalPerson offeror, ILegalPerson offeree)
        {
            if (!base.IsValid(offeror, offeree))
                return false;

            throw new NotImplementedException();
        }
        public override int CompareTo(object obj)
        {
            if (obj is Misdemeanor || obj is Infraction)
                return 1;
            return 0;
        }
    }
}
