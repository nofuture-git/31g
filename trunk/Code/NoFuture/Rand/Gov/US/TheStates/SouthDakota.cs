using System;

namespace NoFuture.Rand.Gov.US.TheStates
{
    [Serializable]
    public class SouthDakota : UsState
    {
        public SouthDakota() : base("SD")
        {
            dlFormats = new[] { new DriversLicense(Numerics(9), this) };
        }
    }
}