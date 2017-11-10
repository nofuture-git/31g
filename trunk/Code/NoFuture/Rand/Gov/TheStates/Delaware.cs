namespace NoFuture.Rand.Gov.TheStates
{
    public class Delaware : UsState
    {
        public Delaware() : base("DE")
        {
            dlFormats = new[] {new DriversLicense(Numerics(7), this) };
        }
    }
}