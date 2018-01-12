namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Oregon : UsState
    {
        public Oregon() : base("OR")
        {
            dlFormats = new[] {new DriversLicense(Numerics(9), this) };
        }
    }
}