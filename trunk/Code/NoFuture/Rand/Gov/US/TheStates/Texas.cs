using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Texas : UsState
    {
        public Texas() : base("TX")
        {
            dlFormats = new[] { new DriversLicense(Numerics(8), this) };
        }
    }
}