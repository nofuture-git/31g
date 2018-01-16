using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Alaska : UsState
    {
        public Alaska() : base("AK")
        {
            dlFormats = new[] { new DriversLicense(Numerics(7), this) };
        }
    }
}