namespace NoFuture.Rand.Gov.TheStates
{
    public class Texas : UsState
    {
        public Texas() : base("TX")
        {
            dlFormats = new[] { new DriversLicense(Numerics(8), this) };
        }
    }
}