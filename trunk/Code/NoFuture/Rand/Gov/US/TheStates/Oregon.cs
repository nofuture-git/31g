using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Oregon : UsState
    {
        public Oregon() : base("OR")
        {
            dlFormats = new[] {new DriversLicense(Numerics(9), this) };
        }
    }
}