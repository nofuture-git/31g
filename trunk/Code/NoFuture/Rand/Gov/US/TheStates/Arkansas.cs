using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Arkansas : UsState
    {
        public Arkansas() : base("AR")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this), new DriversLicense(Numerics(8), this) };
        }
    }
}