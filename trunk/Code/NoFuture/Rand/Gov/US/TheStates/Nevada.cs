namespace NoFuture.Rand.Gov.TheStates
{
    public class Nevada : UsState
    {
        public Nevada() : base("NV")
        {
            dlFormats = new[] {new DriversLicense(Numerics(12), this), new DriversLicense(Numerics(10), this) };
        }
    }
}