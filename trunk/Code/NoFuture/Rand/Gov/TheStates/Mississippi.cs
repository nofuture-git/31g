namespace NoFuture.Rand.Gov.TheStates
{
    public class Mississippi : UsState
    {
        public Mississippi() : base("MS")
        {
            //MS requires this age in order to get married.
            AgeOfMajority = 21;
            dlFormats = new[] { new DriversLicense(Numerics(9), this) };
        }
    }
}