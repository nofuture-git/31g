using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Utah : UsState
    {
        public Utah() : base("UT")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this) };
        }
    }
}