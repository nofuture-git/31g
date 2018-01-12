namespace NoFuture.Rand.Gov.TheStates
{
    public class Alaska : UsState
    {
        public Alaska() : base("AK")
        {
            dlFormats = new[] { new DriversLicense(Numerics(7), this) };
        }
    }
}