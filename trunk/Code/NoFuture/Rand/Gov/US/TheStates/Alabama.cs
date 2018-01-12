namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Alabama : UsState
    {
        public Alabama() : base("AL")
        {
            dlFormats = new[] { new DriversLicense(Numerics(7), this) };
        }
    }
}