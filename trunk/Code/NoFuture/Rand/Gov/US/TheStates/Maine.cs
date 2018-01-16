using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class Maine : UsState
    {
        public Maine() : base("ME")
        {
            dlFormats = new[] { new DriversLicense(Numerics(7), this) };
        }
    }
}