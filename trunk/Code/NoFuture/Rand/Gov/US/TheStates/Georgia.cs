namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Georgia : UsState
    {
        public Georgia() : base("GA")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this) };
        }
    }
}