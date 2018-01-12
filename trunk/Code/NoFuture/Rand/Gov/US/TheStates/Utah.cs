namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Utah : UsState
    {
        public Utah() : base("UT")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this) };
        }
    }
}