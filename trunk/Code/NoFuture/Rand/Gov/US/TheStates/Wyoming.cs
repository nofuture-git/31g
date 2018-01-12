namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Wyoming : UsState
    {
        public Wyoming() : base("WY")
        {
            dlFormats = new[] {new DriversLicense(Numerics(10), this) };
        }
    }
}