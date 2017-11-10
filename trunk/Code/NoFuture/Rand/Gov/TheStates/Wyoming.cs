namespace NoFuture.Rand.Gov.TheStates
{
    public class Wyoming : UsState
    {
        public Wyoming() : base("WY")
        {
            dlFormats = new[] {new DriversLicense(Numerics(10), this) };
        }
    }
}