namespace NoFuture.Rand.Gov.TheStates
{
    public class NorthCarolina : UsState
    {
        public NorthCarolina() : base("NC")
        {
            dlFormats = new[] {new DriversLicense(Numerics(12), this) };
        }
    }
}