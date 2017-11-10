namespace NoFuture.Rand.Gov.TheStates
{
    public class Maine : UsState
    {
        public Maine() : base("ME")
        {
            dlFormats = new[] { new DriversLicense(Numerics(7), this) };
        }
    }
}