using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Georgia : UsState
    {
        public Georgia() : base("GA")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this) };
        }
    }
}