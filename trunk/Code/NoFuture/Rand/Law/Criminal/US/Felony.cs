using System;

namespace NoFuture.Rand.Law.US.Criminal
{
    /// <inheritdoc cref="ICrime"/>
    public class Felony : CrimeBase
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            return true;
        }
        public override int CompareTo(object obj)
        {
            if (obj is Misdemeanor || obj is Infraction)
                return 1;
            return 0;
        }
    }
}
