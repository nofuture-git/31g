namespace NoFuture.Law.Criminal.US
{
    /// <inheritdoc cref="ICrime"/>
    public class Misdemeanor: CrimeBase
    {
        public override bool IsValid(params ILegalPerson[] persons)
        {
            if (!base.IsValid(persons))
                return false;

            return true;
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
