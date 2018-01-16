using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Pennsylvania : UsState
    {
        public Pennsylvania() : base("PA")
        {
            dlFormats = new[] {new DriversLicense(Numerics(8), this) };
        }
    }
}