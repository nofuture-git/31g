namespace NoFuture.Rand.Gov.TheStates
{
    public class SouthCarolina : UsState
    {
        public SouthCarolina() : base("SC")
        {
            dlFormats = new[] { new DriversLicense(Numerics(10), this) };
        }
    }
}