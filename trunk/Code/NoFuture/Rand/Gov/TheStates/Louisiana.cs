namespace NoFuture.Rand.Gov.TheStates
{
    public class Louisiana : UsState
    {
        public Louisiana() : base("LA")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this) };
        }
    }
}