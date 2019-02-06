using System;

namespace NoFuture.Rand.Law.US.Criminal
{
    /// <inheritdoc cref="ICrime"/>
    public class Misdemeanor: CrimeBase
    {
        public override bool IsValid(ILegalPerson offeror = null, ILegalPerson offeree = null)
        {
            if (!base.IsValid(offeror, offeree))
                return false;

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
    }
}
