namespace NoFuture.Rand.Gov.TheStates
{
    public class NewMexico : UsState
    {
        public NewMexico() : base("NM")
        {
            dlFormats = new[] {new DriversLicense(Numerics(9), this), new DriversLicense(Numerics(8), this) };
        }
    }
}