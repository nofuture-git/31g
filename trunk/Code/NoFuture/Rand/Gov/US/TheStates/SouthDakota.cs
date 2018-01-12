namespace NoFuture.Rand.Gov.TheStates
{
    public class SouthDakota : UsState
    {
        public SouthDakota() : base("SD")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this) };
        }
    }
}