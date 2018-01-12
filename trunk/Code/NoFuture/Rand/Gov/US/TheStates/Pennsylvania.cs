namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Pennsylvania : UsState
    {
        public Pennsylvania() : base("PA")
        {
            dlFormats = new[] {new DriversLicense(Numerics(8), this) };
        }
    }
}