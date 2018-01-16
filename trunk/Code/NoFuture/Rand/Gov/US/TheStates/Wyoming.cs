using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Wyoming : UsState
    {
        public Wyoming() : base("WY")
        {
            dlFormats = new[] {new DriversLicense(Numerics(10), this) };
        }
    }
}