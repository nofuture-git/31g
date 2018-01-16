using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class NorthCarolina : UsState
    {
        public NorthCarolina() : base("NC")
        {
            dlFormats = new[] {new DriversLicense(Numerics(12), this) };
        }
    }
}