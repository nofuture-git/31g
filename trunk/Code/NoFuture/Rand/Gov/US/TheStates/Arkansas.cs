namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Arkansas : UsState
    {
        public Arkansas() : base("AR")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this), new DriversLicense(Numerics(8), this) };
        }
    }
}