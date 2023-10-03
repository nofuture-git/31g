using NoFuture.Law.Attributes;

namespace NoFuture.Law.Criminal.US
{
    /// <inheritdoc cref="ICrime"/>
    [Aka("violation")]
    public class Infraction: CrimeBase
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;
            return true;
        }

        public override int CompareTo(object obj)
        {
            if (obj is Felony || obj is Misdemeanor)
                return -1;
            return 0;
        }
    }
}
