namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Delaware : UsState
    {
        public Delaware() : base("DE")
        {
            dlFormats = new[] {new DriversLicense(Numerics(7), this) };
        }
    }
}