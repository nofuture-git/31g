namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Texas : UsState
    {
        public Texas() : base("TX")
        {
            dlFormats = new[] { new DriversLicense(Numerics(8), this) };
        }
    }
}