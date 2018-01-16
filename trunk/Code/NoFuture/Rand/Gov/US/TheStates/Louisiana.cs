using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Louisiana : UsState
    {
        public Louisiana() : base("LA")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this) };
        }
    }
}