namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Alaska : UsState
    {
        public Alaska() : base("AK")
        {
            dlFormats = new[] { new DriversLicense(Numerics(7), this) };
        }
    }
}