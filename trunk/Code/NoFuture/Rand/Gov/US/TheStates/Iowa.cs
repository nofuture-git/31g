using NoFuture.Rand.Core;

namespace NoFuture.Rand.Gov.US.TheStates
{
    public class Iowa : UsState
    {
        public Iowa() : base("IA")
        {
            var dl = new Rchar[9];
            for (var i = 0; i < dl.Length; i++)
            {
                dl[i] = new RcharAlphaNumeric(i);
            }

            dlFormats = new[] {new DriversLicense(dl, this), new DriversLicense(Numerics(9), this) };
        }
    }
}